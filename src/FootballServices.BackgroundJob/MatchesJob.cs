using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.BackgroundJob
{
    public class MatchesJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.FromResult(0);
        }
    }
}
