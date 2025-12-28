using Academic.Domain.Enums;
using MediatR;
using System;

namespace Academic.Application.Features.Classes.Commands
{
    /// <summary>
    /// Command to update the details of an existing academic class.
    /// </summary>
    public class UpdateClassCommand : IRequest<Unit> // IRequest<Unit> means the handler returns nothing (void)
    {
        // Must include the ID of the entity to update
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public StudentClass Level { get; set; }
        public string Stream { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public Guid? FormTeacherId { get; set; }
    }
}