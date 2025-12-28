using Academic.Domain.Common;
using Academic.Domain.Interfaces;
using Academic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Academic.Infrastructure.Persistence.Repositories
{
    // Implementation for the generic contract
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AcademicDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AcademicDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>(); // Uses DbContext to get the DbSet for the specific entity type T
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }
    }
}