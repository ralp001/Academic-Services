using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using Academic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Academic.Infrastructure.Persistence.Repositories
{

    public class ClassSubjectRepository : GenericRepository<ClassSubject>, IClassSubjectRepository
    {
        private readonly AcademicDbContext _dbContext;

        // Note: The GenericRepository constructor is called here.
        public ClassSubjectRepository(AcademicDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<bool> IsAssignedAsync(Guid classId, Guid subjectId)
        {
            // We use the composite key (ClassId and SubjectId) to check for existence.
            return await _dbContext.ClassSubjects
                .AsNoTracking() // Read-only query is faster
                .AnyAsync(cs => cs.ClassId == classId && cs.SubjectId == subjectId);
        }
        public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId)
        {
            // Use the ClassSubjects join table to link to Subjects and filter by ClassId
            return await _dbContext.ClassSubjects
                .AsNoTracking()
                .Where(cs => cs.ClassId == classId)
                .Select(cs => cs.Subject) // Select only the Subject entity
                .Distinct() // Ensure no duplicates if logic allowed it (though unlikely with this query)
                .ToListAsync();
        }
    }
}