using Academic.Domain.Common;

namespace Academic.Domain.Entities
{
    /// <summary>
    /// Represents the many-to-many join table between an academic Class and a Subject.
    /// </summary>
    public class ClassSubject : BaseEntity
    {
        // Composite Primary Key (PK) components
        public Guid ClassId { get; set; }
        public Guid SubjectId { get; set; }

        // Navigation Properties (Required for EF Core to link the tables)
        public Class Class { get; set; } = null!;
        public Subject Subject { get; set; } = null!;

        // Optional: Additional properties for this specific relationship (e.g., IsCompulsory, MaxScore)
        public bool IsCompulsory { get; set; }
        public int WeeklyHours { get; set; } // The number of periods/hours allocated per week
    }
}