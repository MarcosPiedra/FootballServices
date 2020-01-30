using FoorballServices.SqlDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.SqlDataAccess
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly FootballServicesContext dbContext;

        public EFRepository(FootballServicesContext dbContext)
        {
            this.dbSet = dbContext.Set<T>();
            this.dbContext = dbContext;
        }

        public void Add(T item) => dbSet.Add(item);
        public void Remove(T item) => dbSet.Remove(item);
        public IQueryable<T> Query => dbSet.AsNoTracking();
        public void Update(T item) => dbSet.Update(item);
        public T Find(object id) => dbSet.Find(id);
        public T Find(object id1, object id2) => dbSet.Find(id1, id2);
        public async Task SaveAsync() => await dbContext.SaveChangesAsync();
    }
}
