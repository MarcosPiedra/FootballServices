﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.DTOs
{
    public class PlayerRequest
    {
        public string Name { get; set; } = "";
        public int Number { get; set; } = 0;
        public string TeamName { get; set; } = "";
        public int YellowCards { get; set; } = 0;
        public int RedCards { get; set; } = 0;
        public int MinutesPlayed { get; set; } = 0;
    }
}
