using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PostConferenceDAL.Interfaces
{
    public interface IGenericRepository<T>
        where T : class
    {

        Task<IEnumerable<T>> GetAllAsync();

        Task<EntityEntry> PostAsync(T value);

        Task<T> GetAsync(object id);

        Task DeleteAsync(object id);

    }
}
