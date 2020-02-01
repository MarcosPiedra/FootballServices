using FootballServices.Domain.DTOs;
using FootballServices.Domain.Models;
using FootballServices.WebAPI.Tests.Unit;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoorballServices.WebAPI.Tests.Unit
{
    public class StatisticsTest : FootballWebApi
    {
        private static string ManagerMethods => "api/v1/Manager";
        private static string StatisticsMethods => "api/v1/Statistics";
        private ManagerRequest GetManagerToSend() => new ManagerRequest() { Name = "Manager", TeamName = "Team Name 1" };


        [Fact]
        public async Task Statistics_manager_ok()
        {
            var managerToSend = GetManagerToSend();
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(managerToSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ManagerMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (!int.TryParse(idQuery, out int id))
            {
                throw new Exception("Id not found");
            }

            managerToSend.RedCards += 4;
            managerToSend.YellowCards += 5;
            content = new StringContent(JsonConvert.SerializeObject(managerToSend), Encoding.UTF8, "application/json");
            await server.GetTestClient().PutAsync($@"{ManagerMethods}\{id}", content);

            managerToSend.RedCards += 6;
            managerToSend.YellowCards += 7;
            content = new StringContent(JsonConvert.SerializeObject(managerToSend), Encoding.UTF8, "application/json");
            await server.GetTestClient().PutAsync($@"{ManagerMethods}\{id}", content);

            response = await client.GetAsync($"{StatisticsMethods}/yellowcards");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

        }

    }
}

