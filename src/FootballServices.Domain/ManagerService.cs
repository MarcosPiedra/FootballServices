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
    public class ManagerService : IManagerService
    {
        private readonly IRepository<Manager> repository;
        private readonly IStatisticsService statisticsService;

        public ManagerService(IRepository<Manager> repository, 
                              IStatisticsService statisticsService)
        {
            this.repository = repository;
            this.statisticsService = statisticsService;
        }

        public async Task AddAsync(Manager manager)
        {
            await repository.AddAsync(manager);
            await repository.SaveAsync();

            await this.statisticsService.UpdateAsync(manager);
        }

        public async Task<Manager> FindAsync(int id) => await repository.FindAsync(id);

        public async Task<List<Manager>> GetAllAsync() => await repository.Query.ToListAsync();

        public async Task RemoveAsync(Manager manager)
        {
            repository.Remove(manager);
            await repository.SaveAsync();
        }

        public async Task UpdateAsync(Manager manager)
        {
            var oldManager = repository.Query.FirstOrDefault(m => m.Id == manager.Id);

            repository.Update(manager);
            await repository.SaveAsync();

            await this.statisticsService.UpdateAsync(manager, oldManager);
        }
    }
}
