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
    public class PlayerTest : FootballWebApi
    {
        private static string PlayerMethods => "api/v1/Player";
        private PlayerRequest GetToSend() => new PlayerRequest() { Name = "Player", TeamName = "Team Name 1", YellowCards = 1, MinutesPlayed = 5, Number = 10, RedCards = 1 };
        private void ChangeToSend(PlayerRequest m) => m.TeamName = "Team name X";

        [Fact]
        public async Task Get_all_return_ok()
        {
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{PlayerMethods}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_ok()
        {
            var id = 1;
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{PlayerMethods}/{id}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_not_found()
        {
            var id = int.MaxValue;
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{PlayerMethods}/{id}");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{PlayerMethods}", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{PlayerMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                response = await server.GetTestClient().DeleteAsync($@"{PlayerMethods}\{id}");

                Assert.True(response.StatusCode == HttpStatusCode.OK);
            }
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task Put_manager_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{PlayerMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                ChangeToSend(toSend);
                content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
                response = await server.GetTestClient().PutAsync($@"{PlayerMethods}\{id}", content);

                Assert.True(response.StatusCode == HttpStatusCode.OK);
            }
            response.EnsureSuccessStatusCode();
        }

    }
}

