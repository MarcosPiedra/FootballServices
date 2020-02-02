using FluentValidation;
using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FootballServices.Domain.Validation
{
    public class ManagerValidator : AbstractValidator<Manager>
    {
        private readonly IRepository<Manager> repository;

        public ManagerValidator(IRepository<Manager> repository)
        {
            this.repository = repository;

            RuleFor(m => m.Name).NotEmpty().WithMessage("Manager name is required");
            RuleFor(m => m.TeamName).NotEmpty().WithMessage("TeamName name is required");
            RuleFor(m => m).Must(m => NotExistsInOtherTeam(m)).WithMessage("There are other team with the same manager");
        }

        public bool NotExistsInOtherTeam(Manager manager)
        {
            var q = this.repository.Query;

            if (manager.Id > 0)
                q = q.Where(r => r.Id != manager.Id);

            return q.Count(r => r.TeamName == manager.TeamName) == 0;
        }
    }
}
