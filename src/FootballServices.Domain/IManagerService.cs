using FootballServices.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServices.Domain
{
    public interface IManagerService
    {
        Task UpdateAsync(Manager manager);
        Task AddAsync(Manager manager);
        Task RemoveAsync(Manager manager);
        Task<Manager> FindAsync(int id);
        Task<List<Manager>> GetAllAsync();
    }
}