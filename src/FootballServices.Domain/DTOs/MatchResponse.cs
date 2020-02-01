using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Domain.DTOs
{
    public class MatchResponse
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public List<PlayerResponse> HouseTeamPlayers { get; set; } = new List<PlayerResponse>();
        public List<PlayerResponse> AwayTeamPlayers { get; set; } = new List<PlayerResponse>();
        public ManagerResponse HouseTeamManager { get; set; } = new ManagerResponse();
        public ManagerResponse AwayTeamManager { get; set; } = new ManagerResponse();
        public RefereeResponse Referee { get; set; } = new RefereeResponse();
        public DateTime Date { get; set; }
    }
}
