using FootballServices.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServices.Domain
{
    public interface IPlayerService
    {
        Task UpdateAsync(Player player);
        Task AddAsync(Player player);
        Task RemoveAsync(Player player);
        Task<Player> FindAsync(int id);
        Task<List<Player>> GetAllAsync();
    }
}