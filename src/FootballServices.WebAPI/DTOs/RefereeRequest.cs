﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.WebAPI.DTOs
{
    public class RefereeRequest
    {
        public string Name { get; set; } = "";
        public int MinutesPlayed { get; set; } = 0;
    }
}
