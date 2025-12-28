using Academic.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Academic.Domain.Interfaces
{
    public interface IClassSubjectRepository : IGenericRepository<ClassSubject>
    {
        /// <summary>
        /// Checks if a specific subject is already assigned to a class.
        /// </summary>
        Task<bool> IsAssignedAsync(Guid classId, Guid subjectId);
    }
}