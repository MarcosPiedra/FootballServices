using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballServices.Domain
{
    public interface IStatisticsService
    {
        Task UpdateAsync<T>(StatisticsType common, T newValues, T oldValues = null) where T : class;
        Task<List<Statistic>> GetAsync(StatisticsType type);
    }
}
