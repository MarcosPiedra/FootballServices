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

    class NotValidBeforeStart
    {
        public int Id { get; set; } = 0;
        public int Type { get; set; } = 0;
        public RelatedType IdRelated { get; set; } = RelatedType.Player;
        public int MatchId { get; set; } = 0;
    }
}
