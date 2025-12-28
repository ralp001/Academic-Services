using MediatR;
using System;

namespace Academic.Application.Features.Subjects.Queries
{
    /// <summary>
    /// Query to retrieve a single academic subject by its unique ID.
    /// </summary>
    public class GetSubjectByIdQuery : IRequest<SubjectDto> // Returns a single SubjectDto
    {
        public Guid Id { get; set; }

        public GetSubjectByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}