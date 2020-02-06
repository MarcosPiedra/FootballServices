using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.WebAPI.DTOs
{
    public class RefereeResponse
    {
        public string Name { get; set; } = "";
        public int Id { get; set; } = 0;
        public int MinutesPlayed { get; set; } = 0;
    }
}
