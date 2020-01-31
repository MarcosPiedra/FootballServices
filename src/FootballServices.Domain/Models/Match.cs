using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.Models
{
    public enum MatchStatus
    {
        NoPlayed = 0,
        Playing = 1,
        Finished = 1,
    }

    public class Match
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public int HouseTeamPlayersId { get; set; } = 0;
        public int AwayTeamPlayersId { get; set; } = 0;
        public int HouseTeamManagerId { get; set; } = 0;
        public int AwayTeamManager { get; set; } = 0;
        public int RefereeId { get; set; } = 0;
        public MatchStatus Status { get; set; } = MatchStatus.NoPlayed;
        public DateTime Date { get; set; }
    }
}
