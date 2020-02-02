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

namespace FoorballServices.WebAPI.Tests.Unit
{
    [Collection("Sequential")]
    public class JobTest : FootballWebApi
    {
        [Fact]
        public async Task Execute_job()
        {
            var server = await GetServer();

            var job = server.Services.GetService(typeof(IJob)) as IJob;

            await job.Execute(null);
        }
    }
}

