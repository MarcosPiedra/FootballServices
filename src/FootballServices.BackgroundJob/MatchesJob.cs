using FootballServices.Domain;
using Quartz;
using System;
using System.Collections.Generic;
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

            


            await Task.FromResult(0);
        }
    }
}
