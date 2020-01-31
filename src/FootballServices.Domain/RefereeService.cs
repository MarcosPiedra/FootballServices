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

        public RefereeService(IRepository<Referee> repository)
        {
            this.repository = repository;
        }

        public async Task AddAsync(Referee referee)
        {
            await repository.AddAsync(referee);
            await repository.SaveAsync();
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
            repository.Update(referee);
            await repository.SaveAsync();
        }
    }
}
