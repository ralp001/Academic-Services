using MediatR;
using System;

namespace Academic.Application.Features.Subjects.Commands
{
    /// <summary>
    /// Command to logically delete (soft delete) an existing academic subject by ID.
    /// </summary>
    public class DeleteSubjectCommand : IRequest<Unit> // Returns nothing upon success
    {
        public Guid Id { get; set; }

        public DeleteSubjectCommand(Guid id)
        {
            Id = id;
        }
    }
}