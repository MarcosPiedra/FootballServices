using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeamName { get; set; }
        public int YellowCards { get; set; }
    }
}
