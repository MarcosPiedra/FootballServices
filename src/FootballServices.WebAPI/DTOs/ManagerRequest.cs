using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.WebAPI.DTOs
{
    public class ManagerRequest
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string TeamName { get; set; } = "";
        public int YellowCards { get; set; } = 0;
        public int RedCards { get; set; } = 0;
    }
}
