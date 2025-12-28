using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using Academic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Academic.Infrastructure.Persistence.Repositories
{
    // Inherits from the GenericRepository to satisfy IGenericRepository<Subject>
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly AcademicDbContext _dbContext;

        // Pass the DbContext up to the base GenericRepository constructor
        public SubjectRepository(AcademicDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Subject> GetByNameAsync(string subjectName)
        {
            // Use AsNoTracking for read-only query and ToLower() for case-insensitive matching
            return await _dbContext.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name.ToLower() == subjectName.ToLower());
        }
        public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId)
        {
            // The query joins Subjects with ClassSubjects and filters by the provided ClassId.
            // .Select(cs => cs.Subject) projects the result back to just the Subject entities.
            return await _dbContext.ClassSubjects
                .AsNoTracking()
                .Where(cs => cs.ClassId == classId)
                .Select(cs => cs.Subject)
                .Distinct()
                .ToListAsync();
        }
    }
}