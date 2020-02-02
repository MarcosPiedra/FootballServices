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

        public async Task UpdateAsync<T>(T newValues, T oldValues = null) where T : class
        {
            CreateParams(newValues, oldValues, out StatisticsParams stNewValues, out StatisticsParams stOldValues);

            if (!string.IsNullOrEmpty(stNewValues.Team))
                await UpdateCardsAsync(stNewValues, stOldValues);
        }

        public async Task UpdateAsync<T>(StatisticsType source, T newValues, T oldValues = null) where T : class
        {
            CreateParams(newValues, oldValues, out StatisticsParams stNewValues, out StatisticsParams stOldValues);

            if (!string.IsNullOrEmpty(stNewValues.Team))
                await UpdateCardsAsync(stNewValues, stOldValues);

            await UpdateMinutesAsync(source, stNewValues, stOldValues);
        }

        private void CreateParams<T>(T newValues,
                                     T oldValues,
                                     out StatisticsParams stNewValues,
                                     out StatisticsParams stOldValues) where T : class
        {
            stNewValues = new StatisticsParams();
            stOldValues = new StatisticsParams();

            switch (newValues)
            {
                case Manager m:
                    stNewValues = CreateStatisticsParams(m);
                    if (oldValues != null)
                        stOldValues = CreateStatisticsParams(oldValues as Manager);

                    break;
                case Player p:
                    stNewValues = CreateStatisticsParams(p);
                    if (oldValues != null)
                        stOldValues = CreateStatisticsParams(oldValues as Player);

                    break;
                case Referee r:
                    stNewValues = CreateStatisticsParams(r);
                    if (oldValues != null)
                        stOldValues = CreateStatisticsParams(oldValues as Referee);

                    break;
                default:
                    throw new Exception("Canno't convert the type of statistic");
            }
        }

        private async Task UpdateCardsAsync(StatisticsParams newValues, StatisticsParams oldValues)
        {
            if (oldValues == null)
                oldValues = new StatisticsParams();

            var redCards = Math.Max(newValues.RedCards - oldValues.RedCards, 0);
            var yellowCards = Math.Max(newValues.YellowCards - oldValues.YellowCards, 0);
            var team = newValues.Team;

            await this.UpdateAsync(StatisticsType.RedCardsByTeam, redCards, team);
            await this.UpdateAsync(StatisticsType.YellowCardsByTeam, yellowCards, team);
        }

        private async Task UpdateMinutesAsync(StatisticsType common, StatisticsParams newValues, StatisticsParams oldValues)
        {
            if (oldValues == null)
                oldValues = new StatisticsParams();

            var minutes = Math.Max(newValues.Minutes - oldValues.Minutes, 0);

            await UpdateAsync(common, minutes, newValues.Team);
        }

        private async Task UpdateAsync(StatisticsType type, int valueToAdd, string team = "")
        {
            var st = await this.repository
                               .Query
                               .FirstOrDefaultAsync(s => s.Type.Equals(type));

            if (st == null)
            {
                st = new Statistic();
                st.Type = type;
                st.Name = type.ToString();
                st.TeamName = team;
                await this.repository.AddAsync(st);
                await this.repository.SaveAsync();
            }

            st.Name = type.ToString();
            st.Total += valueToAdd;

            this.repository.Update(st);
            await this.repository.SaveAsync();
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
