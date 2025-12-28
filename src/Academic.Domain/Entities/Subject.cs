using Academic.Domain.Common;

namespace Academic.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // e.g., "Mathematics", "Igbo Language"
        public string Description { get; set; } = string.Empty;

        // This links to the Staff ID from the Identity/Onboarding Service
        public Guid? LeadTeacherId { get; set; }
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    }
}