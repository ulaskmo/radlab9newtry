/*using Microsoft.EntityFrameworkCore;
using Rad302SampleExam2024.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rad302GenericDataService
{
    public class GenericDataService<T> : IGenericDataService<T> where T : class
    {
        private readonly ProgrammeDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericDataService(ProgrammeDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> UpdateAsync(string id, T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}*/
