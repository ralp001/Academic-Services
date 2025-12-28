using System;

namespace Academic.Application.Features.Subjects.Queries
{
    /// <summary>
    /// Data Transfer Object used to present Subject details in API responses.
    /// </summary>
    public class SubjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? LeadTeacherId { get; set; }
    }
}