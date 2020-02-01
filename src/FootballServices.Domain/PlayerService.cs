using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballServices.Domain
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<Player> repository;
        private readonly IStatisticsService statisticsService;

        public PlayerService(IRepository<Player> repository,
                             IStatisticsService statisticsService)
        {
            this.repository = repository;
            this.statisticsService = statisticsService;
        }

        public async Task AddAsync(Player player)
        {
            await repository.AddAsync(player);
            await repository.SaveAsync();

            await this.statisticsService.UpdateAsync(StatisticsType.MinutesPlayedByAllPlayers, player);
        }

        public async Task<Player> FindAsync(int id) => await repository.FindAsync(id);

        public async Task<List<Player>> GetAllAsync() => await repository.Query.ToListAsync();

        public async Task RemoveAsync(Player player)
        {
            repository.Remove(player);
            await repository.SaveAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            var oldPlayer = repository.Query.FirstOrDefault(p => p.Id == player.Id);

            repository.Update(player);
            await repository.SaveAsync();

            await this.statisticsService.UpdateAsync(StatisticsType.MinutesPlayedByAllPlayers, player, oldPlayer);
        }
    }
}
