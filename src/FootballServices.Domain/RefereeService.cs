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
    public class RefereeService : IRefereeService
    {
        private readonly IRepository<Referee> repository;
        private readonly IStatisticsService statisticsService;

        public RefereeService(IRepository<Referee> repository,
                              IStatisticsService statisticsService)
        {
            this.repository = repository;
            this.statisticsService = statisticsService;
        }

        public async Task AddAsync(Referee referee)
        {
            await repository.AddAsync(referee);
            await repository.SaveAsync();

            await this.statisticsService.UpdateAsync(StatisticsType.MinutesPlayedByAllReferee, referee);
        }

        public async Task<Referee> FindAsync(int id) => await repository.FindAsync(id);

        public async Task<List<Referee>> GetAllAsync() => await repository.Query.ToListAsync();

        public async Task RemoveAsync(Referee referee)
        {
            repository.Remove(referee);
            await repository.SaveAsync();
        }

        public async Task UpdateAsync(Referee referee)
        {
            var oldReferee = await this.repository.Query.FirstOrDefaultAsync(r => r.Id == referee.Id);

            repository.Update(referee);
            await repository.SaveAsync();

            await this.statisticsService.UpdateAsync(StatisticsType.MinutesPlayedByAllReferee, referee, oldReferee);
        }
    }
}
