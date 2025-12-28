using MediatR;
using System;

namespace Academic.Application.Features.Subjects.Commands
{
    /// <summary>
    /// Command to create a new academic subject.
    /// </summary>
    public class CreateSubjectCommand : IRequest<Guid> // Returns the new Subject Id
    {
        public string Name { get; set; } = string.Empty; // e.g., "Mathematics", "Biology"
        public string Description { get; set; } = string.Empty;

        // The ID of the staff member (Lead Teacher) assigned to coordinate the subject
        public Guid? LeadTeacherId { get; set; }
    }
}