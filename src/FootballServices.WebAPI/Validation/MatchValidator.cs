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
    public class MatchValidator : AbstractValidator<MatchRequest>
    {
        private readonly IRepository<Match> repoMatch;
        private readonly IRepository<Manager> repoManager;
        private readonly IRepository<Player> repoPlayer;

        public MatchValidator(IRepository<Match> repoMatch,
                              IRepository<Manager> repoManager,
                              IRepository<Player> repoPlayer)
        {
            this.repoMatch = repoMatch;
            this.repoManager = repoManager;
            this.repoPlayer = repoPlayer;

            RuleFor(m => m.Name).NotEmpty().WithMessage("Match name is required");
            RuleFor(m => m).Must(m => NotExistsMatchName(m)).WithMessage("The match name is used");
            RuleFor(m => m).Must(m => ExistsManager(m.AwayManager)).WithMessage("The manager away not exits");
            RuleFor(m => m).Must(m => ExistsThePlayer(m.AwayTeam)).WithMessage("There are player that not exists in away team");
            RuleFor(m => m).Must(m => ExistsManager(m.HouseManager)).WithMessage("The house manager not exits");
            RuleFor(m => m).Must(m => ExistsThePlayer(m.HouseTeam)).WithMessage("There are player that not exists in house team");
        }

        private bool ExistsThePlayer(List<int> idList)
        {
            foreach (var id in idList)
            {
                if (this.repoPlayer.Query.Count(m => m.Id == id) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ExistsManager(int id)
        {
            return repoManager.Query.Count(m => m.Id == id) == 1;
        }

        public bool NotExistsMatchName(MatchRequest match)
        {
            var q = this.repoMatch.Query;

            if (match.Id > 0)
                q = q.Where(r => r.Id != match.Id);

            return q.Count(r => r.Name == match.Name) == 0;
        }
    }
}
