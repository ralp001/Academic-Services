using MediatR;
using System;

namespace Academic.Application.Features.ClassSubjects.Commands
{
    /// <summary>
    /// Command to assign a subject to a specific class.
    /// </summary>
    public class AssignSubjectToClassCommand : IRequest<Unit>
    {
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }
        public bool IsCompulsory { get; set; }
        public int WeeklyHours { get; set; } // e.g., 4 periods per week
    }
}