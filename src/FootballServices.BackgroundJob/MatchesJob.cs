using FootballServices.Configurations;
using FootballServices.Domain;
using FootballServices.Domain.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.BackgroundJob
{
    public class MatchesJob : ISpecificJob
    {
        private readonly JobConfiguration jobConfiguration;
        private readonly IMatchService matchService;
        private readonly IRepository<NotValidBeforeStart> notValidRepository;
        private readonly IIncorrectAligmentEndPoint incorrectAligmentEndPoint;

        public MatchesJob(JobConfiguration jobConfiguration,
                          IMatchService matchService,
                          IRepository<NotValidBeforeStart> notValidRepository,
                          IIncorrectAligmentEndPoint incorrectAligmentEndPoint)
        {
            this.jobConfiguration = jobConfiguration;
            this.matchService = matchService;
            this.notValidRepository = notValidRepository;
            this.incorrectAligmentEndPoint = incorrectAligmentEndPoint;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await this.matchService.UpdateStatusAsync();

            var matches = await this.matchService.GetMatchThatStartInAsync(this.jobConfiguration.MinutesBeforeStartMatch);
            foreach (var m in matches)
            {
                var playersNotValids = new List<int>();
                var playersToAdd = m.HouseTeamPlayers
                                    .Where(p => p.RedCards >= 1 || p.YellowCards >= 5)
                                    .Select(p => p.Id);
                playersNotValids.AddRange(playersToAdd.AsEnumerable());
                playersToAdd = m.AwayTeamPlayers
                                .Where(p => p.RedCards >= 1 || p.YellowCards >= 5)
                                .Select(p => p.Id);
                playersNotValids.AddRange(playersToAdd.AsEnumerable());

                var managersNotValids = new List<int>();
                if (m.AwayTeamManager.RedCards >= 1 || m.AwayTeamManager.YellowCards >= 5)
                {
                    managersNotValids.Add(m.AwayTeamManager.Id);
                }
                if (m.HouseTeamManager.RedCards >= 1 || m.HouseTeamManager.YellowCards >= 5)
                {
                    managersNotValids.Add(m.HouseTeamManager.Id);
                }

                var idsNotValids = new List<int>();
                idsNotValids.AddRange(await AddNotValidsAsync(playersNotValids, RelatedType.Player, m.Id));
                idsNotValids.AddRange(await AddNotValidsAsync(managersNotValids, RelatedType.Manager, m.Id));

                if (idsNotValids.Count > 1)
                {
                    await incorrectAligmentEndPoint.Send(idsNotValids);
                    m.IdsIncorrectReported = true;
                    await this.matchService.UpdateAsync(m);
                }
            }
        }

        private async Task<List<int>> AddNotValidsAsync(List<int> idsRelatedValids, RelatedType relatedType, int matchId)
        {
            var idsNotValids = new List<int>();

            foreach (var idRelated in idsRelatedValids)
            {
                var notValid = new NotValidBeforeStart();
                notValid.RelatedId = idRelated;
                notValid.RelatedType = relatedType;
                notValid.MatchId = matchId;

                await this.notValidRepository.AddAsync(notValid);
                await this.notValidRepository.SaveAsync();

                idsNotValids.Add(notValid.Id);
            }

            return idsNotValids;
        }
    }
}
