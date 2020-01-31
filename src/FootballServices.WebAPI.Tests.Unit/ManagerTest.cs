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
            var manager = new ManagerRequest() { Name = "Manager", TeamName = "Team Name 1", YellowCards = 1 };
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(manager), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ManagerMethods}", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_manager_not_content()
        {
            var manager = new ManagerRequest() { Name = "Manager", TeamName = "Team Name 1", YellowCards = 1 };
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(manager), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ManagerMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                response = await server.GetTestClient().DeleteAsync($@"{ManagerMethods}\{id}");

                Assert.True(response.StatusCode == HttpStatusCode.NoContent);
            }
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task Put_manager_not_content()
        {
            var manager = new ManagerRequest() { Name = "Manager", TeamName = "Team Name 1", YellowCards = 1 };
            var server = await GetAPIAsync();
            var client = server.GetTestClient();
            var content = new StringContent(JsonConvert.SerializeObject(manager), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ManagerMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (int.TryParse(idQuery, out int id))
            {
                manager.TeamName = "Team Name 2";
                content = new StringContent(JsonConvert.SerializeObject(manager), Encoding.UTF8, "application/json");
                response = await server.GetTestClient().PutAsync($@"{ManagerMethods}\{id}", content);

                Assert.True(response.StatusCode == HttpStatusCode.NoContent);
            }
            response.EnsureSuccessStatusCode();
        }
    }
}

