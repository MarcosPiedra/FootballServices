using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballServices.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballServices.Domain
{
    public class StatisticsParams
    {
        public int RedCards { get; set; } = 0;
        public int YellowCards { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        public string Team { get; set; } = "";
    }

    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<Statistic> repository;

        public StatisticsService(IRepository<Statistic> repository)
        {
            this.repository = repository;
        }

        public async Task<List<Statistic>> GetAsync(StatisticsType type)
        {
            return await this.repository.Query.Where(s => s.Type.Equals(type))
                                              .ToListAsync();
        }

        public async Task UpdateAsync<T>(StatisticsType common, T newValues, T oldValues = null) where T : class
        {
            StatisticsParams stOldValues = new StatisticsParams();
            StatisticsParams stNewValues = null;

            switch (newValues)
            {
                case Manager m:
                    stNewValues = CreateStatisticsParams(m);
                    if (oldValues != null)
                        stOldValues = CreateStatisticsParams(m);

                    break;
                case Player p:
                    stNewValues = CreateStatisticsParams(p);
                    if (oldValues != null)
                        stOldValues = CreateStatisticsParams(p);

                    break;
                case Referee r:
                    stNewValues = CreateStatisticsParams(r);
                    if (oldValues != null)
                        stOldValues = CreateStatisticsParams(r);

                    break;
                default:
                    throw new Exception("");
            }

            await UpdateAsync(common, stNewValues, stOldValues);
        }

        private async Task UpdateAsync(StatisticsType common,
                                       StatisticsParams newValues,
                                       StatisticsParams oldValues)
        {
            if (oldValues == null)
                oldValues = new StatisticsParams();

            var redCards = Math.Max(newValues.RedCards - oldValues.RedCards, 0);
            var yellowCards = Math.Max(newValues.YellowCards - oldValues.YellowCards, 0);
            var minutes = Math.Max(newValues.Minutes - oldValues.Minutes, 0);

            var team = oldValues.Team;

            if (!string.IsNullOrEmpty(team))
            {
                await this.UpdateAsync(StatisticsType.RedCardsByTeam, redCards, team);
                await this.UpdateAsync(StatisticsType.YellowCardsByTeam, yellowCards, team);
            }

            await this.UpdateAsync(common, minutes, team);
        }

        private async Task UpdateAsync(StatisticsType type, int valueToAdd, string team = "")
        {
            var st = await this.repository
                               .Query
                               .FirstOrDefaultAsync(s => s.Type.Equals(type) && s.TeamName.Equals(team));

            if (st == null)
            {
                st = new Statistic();
                st.Type = type;
                st.TeamName = team;
                await this.repository.AddAsync(st);
            }

            st.Total += valueToAdd;
            this.repository.Update(st);
        }
        private StatisticsParams CreateStatisticsParams(Player player)
        {
            return new StatisticsParams()
            {
                YellowCards = player.YellowCards,
                RedCards = player.RedCards,
                Minutes = player.MinutesPlayed,
                Team = player.TeamName,
            };
        }
        private StatisticsParams CreateStatisticsParams(Manager manager)
        {
            return new StatisticsParams()
            {
                YellowCards = manager.YellowCards,
                RedCards = manager.RedCards,
                Team = manager.TeamName,
            };
        }
        private StatisticsParams CreateStatisticsParams(Referee referee)
        {
            return new StatisticsParams()
            {
                Minutes = referee.MinutesPlayed,
            };
        }

    }
}
