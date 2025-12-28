using MediatR;
using System;

namespace Academic.Application.Features.Classes.Queries
{
    
    public class GetClassByIdQuery : IRequest<ClassDto> // Returns a single ClassDto
    {
        public Guid Id { get; set; }

        public GetClassByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}