using FootballServices.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.BackgroundJob
{
    public class IncorrectAligmentEndPoint : IIncorrectAligmentEndPoint
    {
        private readonly JobConfiguration jobConfiguration;

        public IncorrectAligmentEndPoint(JobConfiguration jobConfiguration)
        {
            this.jobConfiguration = jobConfiguration;
        }

        public async Task Send(List<int> incorrectIds)
        {
            await Task.FromResult(0);
        }
    }
}
