using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.Models
{
    public class Referee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int MinutesPlayed { get; set; } = 0;
    }
}
