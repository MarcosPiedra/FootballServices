using FootballServices.Domain;
using FootballServices.Domain.DTOs;
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

namespace FoorballServices.WebAPI.Tests.Unit
{
    [Collection("Sequential")]
    public class JobTest : FootballWebApi
    {
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
                    //s.AddTransient<IIncorrectAligmentEndPoint, IncorrectAligmentEndPoint>();
                });

            CreateMatchForReport();
            CreateMatchForReport();

            var job = server.Services.GetService(typeof(IJob)) as IJob;
            await job.Execute(null);

            job = server.Services.GetService(typeof(IJob)) as IJob;
            await job.Execute(null);
        }

        private void CreateMatchForReport()
        {
            
        }
    }
}

