using FootballServices.Configurations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.BackgroundJob
{
    public class IncorrectAligmentEndPoint : IIncorrectAligmentEndPoint
    {
        private readonly JobConfiguration jobConfiguration;
        private readonly ILogger<IncorrectAligmentEndPoint> logger;

        public IncorrectAligmentEndPoint(JobConfiguration jobConfiguration,
                                         ILogger<IncorrectAligmentEndPoint> logger)
        {
            this.jobConfiguration = jobConfiguration;
            this.logger = logger;
        }

        public async Task Send(List<int> incorrectIds)
        {
            using var client = new HttpClient();
            var toSend = JsonConvert.SerializeObject(incorrectIds);
            var content = new StringContent(toSend, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{this.jobConfiguration.IncorrectAligmentEndPoint}", content);
            response.EnsureSuccessStatusCode();

            logger.LogInformation($"Incorrect ids reported: {string.Join(',', incorrectIds.ToArray())}");
        }
    }
}
