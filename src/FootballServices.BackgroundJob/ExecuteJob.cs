using FootballServices.Configurations;
using FootballServices.Domain;
using FootballServices.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.BackgroundJob
{
    public class ExecuteJob : IJob
    {
        private readonly IServiceProvider provider;

        public ExecuteJob(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = provider.CreateScope();
            var job = scope.ServiceProvider.GetService<ISpecificJob>();
            await job.Execute(context);
        }
    }
}
