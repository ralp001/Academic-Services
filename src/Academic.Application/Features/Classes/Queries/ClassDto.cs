using System;

namespace Academic.Application.Features.Classes.Queries
{
    /// <summary>
    /// Data Transfer Object used to present Class details in API responses.
    /// This object contains only the properties the external consumer needs.
    /// </summary>
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "JSS 1 Science"

        // This property will hold the string representation of the StudentClass enum.
        public string Level { get; set; } = string.Empty;

        public string Stream { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public Guid? FormTeacherId { get; set; }
    }
}