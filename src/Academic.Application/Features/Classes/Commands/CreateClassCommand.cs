using Academic.Domain.Enums;
using MediatR;
using System;

namespace Academic.Application.Features.Classes.Commands
{
    
    public class CreateClassCommand : IRequest<Guid> // IRequest<Guid> means the handler returns the new Class Id
    {
        public string Name { get; set; } = string.Empty; // e.g., "JSS 1 Science"

        // Use the strongly-typed enum defined in the Domain layer
        public StudentClass Level { get; set; }

        public string Stream { get; set; } = string.Empty; // e.g., 'A', 'Arts', 'Commercial'
        public int MaxCapacity { get; set; } = 35; // Default max capacity

        // The ID of the staff member assigned as the Form Teacher (optional during creation)
        public Guid? FormTeacherId { get; set; }
    }
}