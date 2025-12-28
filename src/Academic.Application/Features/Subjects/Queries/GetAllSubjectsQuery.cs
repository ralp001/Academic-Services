using MediatR;
using System.Collections.Generic;

namespace Academic.Application.Features.Subjects.Queries
{
    public class GetAllSubjectsQuery : IRequest<List<SubjectDto>>
    {
        
    }
}