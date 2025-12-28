using Academic.Domain.Common;
using Academic.Domain.Enums;
using System.Collections.Generic;

namespace Academic.Domain.Entities
{
    /// <summary>
    /// Represents a specific academic group or stream (e.g., "JSS 1A").
    /// </summary>
    public class Class : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // e.g., "JSS 1A", "SSS 2 Science"
        public StudentClass Level { get; set; } // JSS1, SSS3
        public string Stream { get; set; } = string.Empty; // e.g., 'A', 'Science', 'Arts'
        public int MaxCapacity { get; set; }

        // This links to the Staff ID from the Identity/Onboarding Service
        public Guid? FormTeacherId { get; set; }

        // Navigation Property: Subjects taught in this specific class (e.g., SSS2 Science takes Physics)
        public virtual ICollection<Subject> SubjectsOffered { get; set; } = new HashSet<Subject>();
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    }
}