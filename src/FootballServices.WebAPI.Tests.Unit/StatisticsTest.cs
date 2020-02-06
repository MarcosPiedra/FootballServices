using FootballServices.Domain;
using FootballServices.Domain.Models;
using FootballServices.WebAPI.DTOs;
using FootballServices.WebAPI.Tests.Unit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoorballServices.WebAPI.Tests.Unit
{
    [Collection("Sequential")]
    public class StatisticsTest : FootballWebApi
    {
        private static string ManagerMethods => "api/v1/Manager";
        private static string StatisticsMethods => "api/v1/Statistics";
        private static string PlayersMethods => "api/v1/Player";
        private static string RefereeMethods => "api/v1/Referee";
        private ManagerRequest GetManagerToSend(string name) => new ManagerRequest() { Name = name, TeamName = "Team Name 1" };
        private PlayerRequest GetPlayerToSend(string name) => new PlayerRequest() { Name = name, TeamName = "Team Name 1" };
        private RefereeRequest GetRefereeToSend(string name) => new RefereeRequest() { Name = name };

        /// <summary>
        /// Key => Name of the team
        /// Value => Statistics
        /// </summary>
        private Dictionary<string, StatisticsParams> dic = new Dictionary<string, StatisticsParams>();

        private int totalMinutesByReferee = 0;

        [Fact]
        public async Task Statistics_cards_ok()
        {
            dic.Clear();
            var server = await GetServer();
            var client = server.GetTestClient();

            await CreateTeam(server, client, "team1");
            await CreateTeam(server, client, "team2");

            var response = await client.GetAsync($"{StatisticsMethods}/yellowcards");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var cardResponse = JsonConvert.DeserializeObject<List<CardResponse>>(responseBody);
            foreach (var card in cardResponse)
            {
                Assert.Equal(dic[card.TeamName].YellowCards, card.Total);
            }

            response = await client.GetAsync($"{StatisticsMethods}/redcards");
            response.EnsureSuccessStatusCode();

            responseBody = await response.Content.ReadAsStringAsync();
            cardResponse = JsonConvert.DeserializeObject<List<CardResponse>>(responseBody);
            foreach (var card in cardResponse)
            {
                Assert.Equal(dic[card.TeamName].RedCards, card.Total);
            }
        }

        [Fact]
        public async Task Statistics_minutes_ok()
        {
            dic.Clear();
            totalMinutesByReferee = 0;

            var server = await GetServer();
            var client = server.GetTestClient();

            await CreateTeam(server, client, "team1");
            await CreateTeam(server, client, "team2");
            await CreateReferee(server, client);
            await CreateReferee(server, client);

            var response = await client.GetAsync($"{StatisticsMethods}/minutes");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var minuteResponse = JsonConvert.DeserializeObject<List<MinuteResponse>>(responseBody);
            var totalMinutesByTeams = dic.Values.Sum(s => s.Minutes);
            foreach (var minutes in minuteResponse)
            {
                if (minutes.Name == StatisticsType.MinutesPlayedByAllPlayers.ToString())
                {
                    Assert.Equal(totalMinutesByTeams, minutes.Total);
                    continue;
                }
                if (minutes.Name == StatisticsType.MinutesPlayedByAllReferee.ToString())
                {
                    Assert.Equal(totalMinutesByReferee, minutes.Total);
                    continue;
                }
            }
        }

        private async Task CreateReferee(IHost server, HttpClient client)
        {
            var name = Guid.NewGuid().ToString().Substring(0, 6);
            var referee = GetRefereeToSend(name);
            var content = new StringContent(JsonConvert.SerializeObject(referee), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{RefereeMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (!int.TryParse(idQuery, out int id))
            {
                throw new Exception("Id not found");
            }
            referee.MinutesPlayed = 9;
            totalMinutesByReferee += 9;
            content = new StringContent(JsonConvert.SerializeObject(referee), Encoding.UTF8, "application/json");
            response = await server.GetTestClient().PutAsync($@"{RefereeMethods}\{id}", content);
        }

        private async Task CreateTeam(IHost server, HttpClient client, string team)
        {
            await CreateAndUpdateManager(server, client, team);
            await CreateAndUpdatePlayer(server, client, team);
            await CreateAndUpdatePlayer(server, client, team);
            await CreateAndUpdatePlayer(server, client, team);
        }

        private async Task CreateAndUpdatePlayer(IHost server, HttpClient client, string team)
        {
            var name = Guid.NewGuid().ToString().Substring(0, 6);
            var playerToSend = GetPlayerToSend(name);
            playerToSend.TeamName = team;
            var content = new StringContent(JsonConvert.SerializeObject(playerToSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{PlayersMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (!int.TryParse(idQuery, out int id))
            {
                throw new Exception("Id not found");
            }

            playerToSend.YellowCards += 4;
            playerToSend.RedCards += 5;
            playerToSend.MinutesPlayed += 6;
            UpdateStatistic(team, 4, 5, 6);

            content = new StringContent(JsonConvert.SerializeObject(playerToSend), Encoding.UTF8, "application/json");
            await server.GetTestClient().PutAsync($@"{PlayersMethods}\{id}", content);

            playerToSend.YellowCards += 7;
            playerToSend.RedCards += 8;
            playerToSend.MinutesPlayed += 9;
            UpdateStatistic(team, 7, 8, 9);
            content = new StringContent(JsonConvert.SerializeObject(playerToSend), Encoding.UTF8, "application/json");
            await server.GetTestClient().PutAsync($@"{PlayersMethods}\{id}", content);
        }

        private async Task CreateAndUpdateManager(IHost server, HttpClient client, string team)
        {
            var name = Guid.NewGuid().ToString().Substring(0, 6);
            var managerToSend = GetManagerToSend(name);
            managerToSend.TeamName = team;
            var content = new StringContent(JsonConvert.SerializeObject(managerToSend), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ManagerMethods}", content);
            var idQuery = response.Headers.Location.Query.Replace("?id=", "");
            if (!int.TryParse(idQuery, out int id))
            {
                throw new Exception("Id not found");
            }

            managerToSend.YellowCards += 4;
            managerToSend.RedCards += 5;
            UpdateStatistic(team, 4, 5, 0);
            content = new StringContent(JsonConvert.SerializeObject(managerToSend), Encoding.UTF8, "application/json");
            await server.GetTestClient().PutAsync($@"{ManagerMethods}\{id}", content);

            managerToSend.YellowCards += 6;
            managerToSend.RedCards += 7;
            UpdateStatistic(team, 6, 7, 0);
            content = new StringContent(JsonConvert.SerializeObject(managerToSend), Encoding.UTF8, "application/json");
            await server.GetTestClient().PutAsync($@"{ManagerMethods}\{id}", content);
        }

        private void UpdateStatistic(string team,
                                     int deltaYellowCards,
                                     int deltaRedCards,
                                     int deltaMinutes)
        {
            if (this.dic.ContainsKey(team))
            {
                var vals = this.dic[team];
                vals.YellowCards += deltaYellowCards;
                vals.RedCards += deltaRedCards;
                vals.Minutes += deltaMinutes;
            }
            else
            {
                var vals = new StatisticsParams();
                vals.YellowCards = deltaYellowCards;
                vals.RedCards = deltaRedCards;
                vals.Minutes = deltaMinutes;
                this.dic.Add(team, vals);
            }
        }
    }
}

