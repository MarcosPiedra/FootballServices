using FluentValidation;
using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FootballServices.Domain;
using FootballServices.WebAPI.DTOs;

namespace FootballServices.WebAPI.Validation
{
    public class PlayerValidator : AbstractValidator<PlayerRequest>
    {
        private readonly IRepository<Player> repository;

        public PlayerValidator(IRepository<Player> repository)
        {
            this.repository = repository;

            RuleFor(p => p.Name).NotEmpty().WithMessage("Player name is required");
            RuleFor(p => p.TeamName).NotEmpty().WithMessage("Team name is required");
        }
    }
}
