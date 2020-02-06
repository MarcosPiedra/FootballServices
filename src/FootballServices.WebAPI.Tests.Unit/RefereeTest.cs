using FootballServices.Domain.Models;
using FootballServices.WebAPI.DTOs;
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
    public class RefereeTest : FootballWebApi
    {
        private static string RefereeMethods => "api/v1/Referee";
        private RefereeRequest GetToSend() => new RefereeRequest() { Name = "Referee x", MinutesPlayed = 5 };
        private void ChangeToSend(RefereeRequest m) => m.Name = "Referee 11";

        [Fact]
        public async Task Get_all_return_ok()
        {
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{RefereeMethods}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{RefereeMethods}", content);
            response.EnsureSuccessStatusCode();

            var id = 1;
            response = await client.GetAsync($"{RefereeMethods}/{id}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_not_found()
        {
            var id = int.MaxValue;
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{RefereeMethods}/{id}");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{RefereeMethods}", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{RefereeMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                response = await server.GetTestClient().DeleteAsync($@"{RefereeMethods}\{id}");

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
            var response = await client.PostAsync($"{RefereeMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                ChangeToSend(toSend);
                content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
                response = await server.GetTestClient().PutAsync($@"{RefereeMethods}\{id}", content);

                Assert.True(response.StatusCode == HttpStatusCode.OK);
            }
            response.EnsureSuccessStatusCode();
        }

    }
}

