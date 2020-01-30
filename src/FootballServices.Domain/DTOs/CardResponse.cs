using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.DTOs
{
    public class CardResponse
    {
        public string Name { get; set; } = "";
        public int Id { get; set; } = 0;
        public string TeanName { get; set; } = "";
        public int Total { get; set; } = 0;
    }
}
