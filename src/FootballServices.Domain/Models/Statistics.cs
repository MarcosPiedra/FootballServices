using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.Models
{
    public enum StatisticsType
    {
        YellowCardsByTeam = 0,
        RedCardsByTeam = 1,
        MinutesPlayedByAllReferee = 2,
        MinutesPlayedByAllPlayers = 3
    }

    public class Statistic
    {
        public int Id { get; set; } = 0;
        public StatisticsType Type { get; set; } = StatisticsType.YellowCardsByTeam;
        public string TeamName { get; set; } = "";
        public int Total { get; set; } = 0;
        public string Name { get; internal set; }
    }
}
