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
        }
    }
}
