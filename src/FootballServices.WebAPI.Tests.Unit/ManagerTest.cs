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
    public class ManagerTest : FootballWebApi
    {
        private static string ManagerMethods => "api/v1/Manager";

        [Fact]
        public async Task Get_all_managers_ok()
        {
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{ManagerMethods}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_get_manager_by_id_ok()
        {
            var id = 1;
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{ManagerMethods}/{id}");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_manager_by_id_not_found()
        {
            var id = int.MaxValue;
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var response = await client.GetAsync($"{ManagerMethods}/{id}");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Post_manager_ok()
        {
            var manager = new Manager() { Name = "Manager", TeamName = "Team Name 1", YellowCards = 1 };
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(manager), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ManagerMethods}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
