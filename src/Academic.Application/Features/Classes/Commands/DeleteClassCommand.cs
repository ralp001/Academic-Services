using MediatR;
using System;

namespace Academic.Application.Features.Classes.Commands
{
    /// <summary>
    /// Command to logically delete (soft delete) an existing academic class by ID.
    /// </summary>
    public class DeleteClassCommand : IRequest<Unit> // Returns nothing upon success
    {
        public Guid Id { get; set; }

        public DeleteClassCommand(Guid id)
        {
            Id = id;
        }
    }
}