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
    public class RefereeValidator : AbstractValidator<RefereeRequest>
    {
        private readonly IRepository<Referee> repository;

        public RefereeValidator(IRepository<Referee> repository)
        {
            this.repository = repository;

            RuleFor(r => r.Name).NotEmpty().WithMessage("Referee name is required");
        }
    }
}
