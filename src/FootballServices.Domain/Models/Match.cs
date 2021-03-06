﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.Models
{
    public enum MatchStatus
    {
        NoPlayedYet = 0,
        Playing = 1,
        Finished = 1,
    }

    public class Match
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string HouseTeamPlayersIds { get; set; } = "[0,0,0,0,0,0,0,0,0,0,0]";
        public string AwayTeamPlayersIds { get; set; } = "[0,0,0,0,0,0,0,0,0,0,0]";
        public int HouseTeamManagerId { get; set; } = 0;
        public int AwayTeamManagerId { get; set; } = 0;
        public int RefereeId { get; set; } = 0;
        public MatchStatus Status { get; set; } = MatchStatus.NoPlayedYet;
        public DateTime Date { get; set; }
        public bool IdsIncorrectReported { get; set; } = false;
        public List<Player> HouseTeamPlayers { get; set; } = new List<Player>();
        public List<Player> AwayTeamPlayers { get; set; } = new List<Player>();
        public Manager HouseTeamManager { get; set; } = new Manager();
        public Manager AwayTeamManager { get; set; } = new Manager();
        public Referee Referee { get; set; } = new Referee();
    }
}
