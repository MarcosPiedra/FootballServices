using FootballServices.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServices.Domain
{
    public interface IRefereeService
    {
        Task UpdateAsync(Referee referee);
        Task AddAsync(Referee referee);
        Task RemoveAsync(Referee referee);
        Task<Referee> FindAsync(int id);
        Task<List<Referee>> GetAllAsync();
    }
}