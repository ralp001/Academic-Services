using Academic.Domain.Entities;
using System.Threading.Tasks;

namespace Academic.Domain.Interfaces
{
    // Inherits basic CRUD methods (Add, GetById, Update, Delete, GetAll)
    public interface IClassRepository : IGenericRepository<Class>
    {
        // ONLY define custom queries unique to Class:
        Task<Class> GetByNameAsync(string className);
    }
}