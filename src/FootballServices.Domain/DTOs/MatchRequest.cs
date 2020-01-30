﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.DTOs
{
    public class MatchRequest
    {
        public string Name { get; set; } = "";
        public List<int> HouseTeam { get; set; } = new List<int>();
        public int HouseManager { get; set; } = 0;
        public List<int> AwayTeam { get; set; } = new List<int>();
        public int AwayManager { get; set; } = 0;
        public int Referee { get; set; } = 0;
        public DateTime Date { get; set; }
    }
}
