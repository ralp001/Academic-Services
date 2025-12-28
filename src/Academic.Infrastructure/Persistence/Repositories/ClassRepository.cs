using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using Academic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Academic.Infrastructure.Persistence.Repositories
{
    // Inherits from the GenericRepository to satisfy IGenericRepository<Class>
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        // Private field for the DbContext if needed for complex custom queries, 
        // but often the base class handles the core context access.
        private readonly AcademicDbContext _dbContext;

        // Pass the DbContext up to the base GenericRepository constructor
        public ClassRepository(AcademicDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // --- Custom Query Implementation ---

        /// <summary>
        /// Retrieves a class by its unique name (e.g., "JSS 1A").
        /// This method is specific to IClassRepository and not part of the generic contract.
        /// </summary>
        public async Task<Class> GetByNameAsync(string className)
        {
            // Use AsNoTracking for read-only query and ToLower() for case-insensitive matching
            return await _dbContext.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == className.ToLower());
        }
    }
}