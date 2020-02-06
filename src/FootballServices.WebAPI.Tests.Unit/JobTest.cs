using FootballServices.Domain;
using FootballServices.Domain.Models;
using FootballServices.WebAPI.Tests.Unit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using FootballServices.BackgroundJob;
using FootballServices.Configurations;

namespace FoorballServices.WebAPI.Tests.Unit
{
    [Collection("Sequential")]
    public class JobTest : FootballWebApi
    {
        JobConfiguration jobConfiguration;

        List<int> incorrectPlayersId = new List<int>();
        List<int> incorrectManagersId = new List<int>();

        List<int> incorrectIdReported = new List<int>();

        public class IncorrectAligmentEndPoint : IIncorrectAligmentEndPoint
        {
            public JobTest jobTest;

            public IncorrectAligmentEndPoint(JobTest jobTest)
            {
                this.jobTest = jobTest;
            }

            public async Task Send(List<int> incorrectIds)
            {
                jobTest.incorrectIdReported.AddRange(incorrectIds);
                await Task.FromResult(0);
            }
        }

        [Fact]
        public async Task Execute_job()
        {
            var server = await GetServer(
                s =>
                {
                    s.AddSingleton(this);
                    s.AddTransient<IIncorrectAligmentEndPoint, IncorrectAligmentEndPoint>();
                });

            this.jobConfiguration = server.Services.GetService(typeof(JobConfiguration)) as JobConfiguration;

            // is in the report
            await CreateMatchAsync(server, 5);
            await CreateMatchAsync(server, 20);
            // is not in the report
            await CreateMatchAsync(server, 50); // going to play Match
            await CreateMatchAsync(server, -20); // Match played

            var job = server.Services.GetService(typeof(IJob)) as IJob;
            await job.Execute(null);

            await Task.Delay(1000);

            job = server.Services.GetService(typeof(IJob)) as IJob;
            await job.Execute(null);

            var totalReported = this.incorrectPlayersId.Count + this.incorrectManagersId.Count;
            Assert.Equal(this.incorrectIdReported.Count, totalReported);

            var repo = server.Services.GetService(typeof(IRepository<NotValidBeforeStart>)) as IRepository<NotValidBeforeStart>;
            var incorrectManagersReported = repo.Query.Where(d => d.RelatedType == RelatedType.Manager).Select(d => d.RelatedId).ToList();
            Assert.Equal(incorrectManagersReported.OrderBy(d => d), this.incorrectManagersId.OrderBy(d => d));

            var incorrectPlayersReported = repo.Query.Where(d => d.RelatedType == RelatedType.Player).Select(d => d.RelatedId).ToList();
            Assert.Equal(incorrectPlayersReported.OrderBy(d => d), this.incorrectPlayersId.OrderBy(d => d));
        }

        private async Task CreateMatchAsync(IHost server, int startInMinutes)
        {
            var repoPlayer = server.Services.GetService(typeof(IRepository<Player>)) as IRepository<Player>;
            var repoManager = server.Services.GetService(typeof(IRepository<Manager>)) as IRepository<Manager>;
            var repoMatch = server.Services.GetService(typeof(IRepository<Match>)) as IRepository<Match>;
            var now = DateTime.Now;
            var startDate = now.AddMinutes(startInMinutes);
            var whenStartReport = startDate.AddMinutes(-this.jobConfiguration.MinutesBeforeStartMatch);
            var isInReport = whenStartReport <= now && now <= startDate;

            var p11 = await AddPlayerAsync("Team1", 1, 3, repoPlayer, isInReport);
            var p12 = await AddPlayerAsync("Team1", 0, 0, repoPlayer, isInReport);
            var p13 = await AddPlayerAsync("Team1", 6, 0, repoPlayer, isInReport);
            var m11 = await AddManagerAsync("Team1", 6, 0, repoManager, isInReport);

            var p21 = await AddPlayerAsync("Team2", 0, 0, repoPlayer, isInReport);
            var p22 = await AddPlayerAsync("Team2", 7, 0, repoPlayer, isInReport);
            var p23 = await AddPlayerAsync("Team2", 5, 1, repoPlayer, isInReport);
            var m21 = await AddManagerAsync("Team2", 0, 0, repoManager, isInReport);

            var awayPlayers = $"[{p11},{p12},{p13}]";
            var housePlayers = $"[{p21},{p22},{p23}]";

            await AddMatchAsync(m11, awayPlayers, m21, housePlayers, startDate, repoMatch);
        }

        private async Task<int> AddPlayerAsync(string team, int yellowCards, int redCards, IRepository<Player> repoPlayer, bool isInReport)
        {
            var p = new Player()
            {
                YellowCards = yellowCards,
                RedCards = redCards,
                TeamName = team
            };

            await repoPlayer.AddAsync(p);
            await repoPlayer.SaveAsync();

            if (isInReport)
            {
                if (yellowCards >= 5 || redCards >= 1)
                {
                    incorrectPlayersId.Add(p.Id);
                }
            }

            return p.Id;
        }
        private async Task<int> AddManagerAsync(string team, int yellowCards, int redCards, IRepository<Manager> repoManager, bool isInReport)
        {
            var m = new Manager()
            {
                YellowCards = yellowCards,
                RedCards = redCards,
                TeamName = team
            };

            await repoManager.AddAsync(m);
            await repoManager.SaveAsync();

            if (isInReport)
            {
                if (yellowCards >= 5 || redCards >= 1)
                {
                    incorrectManagersId.Add(m.Id);
                }
            }

            return m.Id;
        }
        private async Task AddMatchAsync(int awaiManagerId,
                                         string awayPlayerIds,
                                         int houseManagerId,
                                         string housePlayerIds,
                                         DateTime date,
                                         IRepository<Match> repotMatch)
        {
            var m = new Match()
            {
                AwayTeamManagerId = awaiManagerId,
                AwayTeamPlayersIds = awayPlayerIds,
                HouseTeamManagerId = houseManagerId,
                HouseTeamPlayersIds = housePlayerIds,
                Date = date,
                Name = Guid.NewGuid().ToString().Substring(0, 10)
            };

            await repotMatch.AddAsync(m);
            await repotMatch.SaveAsync();
        }
    }
}

