using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.Models
{
    public enum RelatedType
    {
        Player = 0,
        Manager = 0,
    }

    public class NotValidBeforeStart
    {
        public int Id { get; set; } = 0;
        public RelatedType RelatedType { get; set; } = RelatedType.Player;
        public int RelatedId { get; set; } = 0;
        public int MatchId { get; set; } = 0;
    }
}
