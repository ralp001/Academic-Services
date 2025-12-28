using MediatR;
using System;

namespace Academic.Application.Features.Subjects.Commands
{
    /// <summary>
    /// Command to update the details of an existing academic subject.
    /// </summary>
    public class UpdateSubjectCommand : IRequest<Unit> // Returns nothing upon success
    {
        // Must include the ID of the entity to update
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? LeadTeacherId { get; set; }
    }
}