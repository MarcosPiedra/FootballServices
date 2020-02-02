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
            RuleFor(r => r).Must(r => NotExistsRefereeName(r)).WithMessage("The referee name is used");
        }

        public bool NotExistsRefereeName(Referee referee)
        {
            var q = this.repository.Query;

            if (referee.Id > 0)
                q = q.Where(r => r.Id != referee.Id);
            
            return q.Count(r => r.Name == referee.Name) == 0;
        }
    }
}
