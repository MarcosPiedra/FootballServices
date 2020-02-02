using FluentValidation;
using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FootballServices.Domain.Validation
{
    public class RefereeValidator : AbstractValidator<Referee>
    {
        private readonly IRepository<Referee> repository;

        public RefereeValidator(IRepository<Referee> repository)
        {
            this.repository = repository;

            RuleFor(r => r.Name).NotEmpty().WithMessage("Referee name is required");
        }
    }
}
