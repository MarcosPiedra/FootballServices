using FootballServices.Domain;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.BackgroundJob
{
    public class MatchesJob : IJob
    {
        private readonly IMatchService matchService;

        public MatchesJob(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await this.matchService.UpdateStatusAsync();

            var matches = await this.matchService.GetMatchThatStartInAsync(5);
            foreach (var m in matches)
            {
                var playersNotValids = new List<int>();
                var playersToAdd = m.HouseTeamPlayers
                                 .Where(p => p.RedCards >= 1 || p.YellowCards >= 5)
                                 .Select(p => p.Id);
                playersNotValids.AddRange(playersToAdd.AsEnumerable());
                playersToAdd = m.AwayTeamPlayers
                             .Where(p => p.RedCards >= 1 || p.YellowCards >= 5)
                             .Select(p => p.Id);
                playersNotValids.AddRange(playersToAdd.AsEnumerable());


            }


            await Task.FromResult(0);
        }
    }
}
