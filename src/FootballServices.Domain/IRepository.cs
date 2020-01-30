using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.Domain
{
    public interface IRepository<T>
    {
        void Update(T entity);
        Task AddAsync(T entity);
        void Remove(T entity);
        Task<T> FindAsync(object id);
        Task<T> FindAsync(object id1, object id2);
        IQueryable<T> Query { get; }
        Task SaveAsync();
    }
}
