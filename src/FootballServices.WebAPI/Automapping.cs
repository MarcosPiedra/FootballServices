using AutoMapper;
using FootballServices.Domain.DTOs;
using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballServices.WebAPI
{
    public class Automapping : Profile
    {
        public Automapping()
        {
            CreateMap<Manager, ManagerResponse>();
            CreateMap<ManagerRequest, Manager>();

            CreateMap<Player, PlayerResponse>();
            CreateMap<PlayerRequest, Player>();

            CreateMap<Referee, RefereeResponse>();
            CreateMap<RefereeRequest, Referee>();

            CreateMap<Match, MatchResponse>();
            CreateMap<MatchRequest, Match>()
                   .ForMember(d => d.Referee, o => o.Ignore())
                   .ForMember(d => d.RefereeId, o => o.MapFrom(s => s.Referee))
                   .ForMember(d => d.AwayTeamPlayersIds, o => o.MapFrom(s => $"[{string.Join(',', s.AwayTeam)}]"))
                   .ForMember(d => d.AwayTeamManagerId, o => o.MapFrom(s => s.AwayManager))
                   .ForMember(d => d.HouseTeamPlayersIds, o => o.MapFrom(s => $"[{string.Join(',', s.HouseTeam)}]"))
                   .ForMember(d => d.HouseTeamManagerId, o => o.MapFrom(s => s.AwayManager));
        }
    }
}
