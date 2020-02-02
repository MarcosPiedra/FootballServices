using FootballServices.Domain.DTOs;
using FootballServices.Domain.Models;
using FootballServices.WebAPI.Tests.Unit;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoorballServices.WebAPI.Tests.Unit
{
    public class MatchTest : FootballWebApi
    {
        private static string MatchMethods => "api/v1/Match";
        private MatchRequest GetToSend() => new MatchRequest()
        {
            AwayManager = 1,
            AwayTeam = new List<int>() { 1, 2 },
            Date = DateTime.Now,
            HouseManager = 1,
            HouseTeam = new List<int>() { 3, 4 },
            Referee = 2,
            Name = "Name XXX"
        };
        private void ChangeToSend(MatchRequest m)
        {
            m.Name = "Name YYY";
            var x = m.AwayManager;
            var y = m.AwayTeam;
            m.AwayManager = m.HouseManager;
            m.HouseManager = x;
            m.AwayTeam = m.HouseTeam;
            m.HouseTeam = y;
            m.Date = DateTime.Now.AddDays(-1);
        }

        [Fact]
        public async Task Get_all_return_ok()
        {
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{MatchMethods}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_ok()
        {
            var id = 1;
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{MatchMethods}/{id}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_by_id_return_not_found()
        {
            var id = int.MaxValue;
            var server = await GetServer();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{MatchMethods}/{id}");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Post_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{MatchMethods}", content);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Delete_return_ok()
        {
            var toSend = GetToSend();
            var server = await GetServer();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{MatchMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                response = await server.GetTestClient().DeleteAsync($@"{MatchMethods}\{id}");

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
            var response = await client.PostAsync($"{MatchMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                ChangeToSend(toSend);
                content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
                response = await server.GetTestClient().PutAsync($@"{MatchMethods}\{id}", content);

                Assert.True(response.StatusCode == HttpStatusCode.OK);
            }
            response.EnsureSuccessStatusCode();
        }
    }
}

