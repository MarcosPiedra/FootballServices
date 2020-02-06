using FootballServices.Domain;
using FootballServices.Domain.Models;
using FootballServices.WebAPI.DTOs;
using FootballServices.WebAPI.Tests.Unit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoorballServices.WebAPI.Tests.Unit
{
    public class MatchTest : FootballWebApi
    {
        private static string MatchMethods => "api/v1/Match";
        private string GetName() => Guid.NewGuid().ToString().Substring(0, 7);
        private MatchRequest GetToSend() => new MatchRequest()
        {
            AwayManager = 1,
            AwayTeam = new List<int>() { 1, 2 },
            Date = DateTime.Now,
            HouseManager = 1,
            HouseTeam = new List<int>() { 3, 4 },
            Referee = 2,
            Name = "Name XXX"
        };
        private void ChangeToSend(MatchRequest m)
        {
            m.Name = "Name YYY";
            var x = m.AwayManager;
            var y = m.AwayTeam;
            m.AwayManager = m.HouseManager;
            m.HouseManager = x;
            m.AwayTeam = m.HouseTeam;
            m.HouseTeam = y;
            m.Date = DateTime.Now.AddDays(-1);
        }

        [Fact]
        public async Task Get_all_return_ok()
        {
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{MatchMethods}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_ok()
        {
            var id = 1;
            var server = await GetServer();
            await CreateMatchAsync(server);
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{MatchMethods}/{id}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_not_found()
        {
            var id = int.MaxValue;
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{MatchMethods}/{id}");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Post_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            await CreateMatchAsync(server);
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{MatchMethods}", content);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Delete_return_ok()
        {
            var server = await GetServer();
            await CreateMatchAsync(server);
            var toSend = GetToSend();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{MatchMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                response = await server.GetTestClient().DeleteAsync($@"{MatchMethods}\{id}");

                Assert.True(response.StatusCode == HttpStatusCode.OK);
            }
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_manager_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            await CreateMatchAsync(server);
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{MatchMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                ChangeToSend(toSend);
                content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
                response = await server.GetTestClient().PutAsync($@"{MatchMethods}\{id}", content);

                Assert.True(response.StatusCode == HttpStatusCode.OK);
            }
            response.EnsureSuccessStatusCode();
        }

        private async Task CreateMatchAsync(IHost server)
        {
            var repoPlayer = server.Services.GetService(typeof(IRepository<Player>)) as IRepository<Player>;
            var repoManager = server.Services.GetService(typeof(IRepository<Manager>)) as IRepository<Manager>;
            var repoMatch = server.Services.GetService(typeof(IRepository<Match>)) as IRepository<Match>;
            var now = DateTime.Now;

            var p11 = await AddPlayerAsync("Team1", repoPlayer);
            var p12 = await AddPlayerAsync("Team1", repoPlayer);
            var p13 = await AddPlayerAsync("Team1", repoPlayer);
            var m11 = await AddManagerAsync("Team1", repoManager);

            var p21 = await AddPlayerAsync("Team2", repoPlayer);
            var p22 = await AddPlayerAsync("Team2", repoPlayer);
            var p23 = await AddPlayerAsync("Team2", repoPlayer);
            var m21 = await AddManagerAsync("Team2", repoManager);

            var awayPlayers = $"[{p11},{p12},{p13}]";
            var housePlayers = $"[{p21},{p22},{p23}]";

            await AddMatchAsync(m11, awayPlayers, m21, housePlayers, now, repoMatch);
        }

        private async Task<int> AddPlayerAsync(string team, IRepository<Player> repoPlayer)
        {
            var p = new Player()
            {
                Name = this.GetName(),
                TeamName = team
            };

            await repoPlayer.AddAsync(p);
            await repoPlayer.SaveAsync();

            return p.Id;
        }
        private async Task<int> AddManagerAsync(string team, IRepository<Manager> repoManager)
        {
            var m = new Manager()
            {
                Name = this.GetName(),
                TeamName = team
            };

            await repoManager.AddAsync(m);
            await repoManager.SaveAsync();

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

