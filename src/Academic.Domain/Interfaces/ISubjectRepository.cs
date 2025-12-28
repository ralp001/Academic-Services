using Academic.Domain.Entities;
using System.Threading.Tasks;

namespace Academic.Domain.Interfaces
{
    // Inherits basic CRUD methods
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        // ONLY define custom queries unique to Subject:
        Task<Subject> GetByNameAsync(string subjectName);
        Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(Guid classId);
    }
}