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
        public async Task Get_get_all_managers()
        {
            try
            {
                var server = await CreateServerAsync();
                var client = server.GetTestClient();
                var response = await client.GetAsync($"{ManagerMethods}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}
