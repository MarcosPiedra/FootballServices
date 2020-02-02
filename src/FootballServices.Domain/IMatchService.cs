using FootballServices.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServices.Domain
{
    public interface IMatchService
    {
        Task UpdateAsync(Match match);
        Task AddAsync(Match match);
        Task RemoveAsync(Match match);
        Task<Match> FindAsync(int id);
        Task<List<Match>> GetAllAsync();
        void UpdateStatus(Match match);
        Task UpdateStatusAsync();
        Task<List<Match>> GetMatchThatStartInAsync(int minutes);
    }
}