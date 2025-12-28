using MediatR;
using System.Collections.Generic;

namespace Academic.Application.Features.Classes.Queries
{
    /// <summary>
    /// Query to retrieve a list of all academic classes.
    /// </summary>
    public class GetAllClassesQuery : IRequest<List<ClassDto>> // Returns a list of our specific DTO
    {
        // No parameters needed for retrieving all records
    }
}