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

        public ManagerService(IRepository<Manager> repository)
        {
            this.repository = repository;
        }

        public void Add(Manager manager) => repository.Add(manager);

        public async Task<List<Manager>> GetAllAsync() => await repository.Query.ToListAsync();

        public void Remove(Manager manager) => repository.Remove(manager);

        public void Update(Manager manager) => repository.Update(manager);
    }
}
