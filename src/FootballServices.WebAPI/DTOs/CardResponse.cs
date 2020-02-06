using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.WebAPI.DTOs
{
    public class CardResponse
    {
        public string Name { get; set; } = "";
        public int Id { get; set; } = 0;
        public string TeamName { get; set; } = "";
        public int Total { get; set; } = 0;
    }
}
