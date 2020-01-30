using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballServices.Domain
{
    public interface IRepository<T>
    {
        void Update(T entity);
        void Add(T entity);
        void Remove(T entity);
        T Find(object id);
        T Find(object id1, object id2);
        IQueryable<T> Query { get; }
    }
}
