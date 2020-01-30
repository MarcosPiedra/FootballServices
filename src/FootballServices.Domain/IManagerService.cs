using FootballServices.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballServices.Domain
{
    public interface IManagerService
    {
        void Update(Manager manager);
        void Add(Manager manager);
        void Remove(Manager manager);
        Task<List<Manager>> GetAllAsync();
    }
}