using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PostConferenceDAL.Interfaces;

namespace PostConferenceDAL.Repository
{
    public class GenericRepository<T, TContext> : IDisposable, IGenericRepository<T>
        where T : class
        where TContext : DbContext
    {


        readonly DbContext _context;

        readonly DbSet<T> _set;

        public GenericRepository(TContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public async Task DeleteAsync(object id)
        {

            var result = await _set.FindAsync(id);
            _context.Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetAsync(object id)
        {
            var result = await _set.FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _set.ToListAsync();
            return result;
        }

        public async Task<EntityEntry> PostAsync(T value)
        {
            var entity = await _set.AddAsync(value);
            await _context.SaveChangesAsync();

            return entity;
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
