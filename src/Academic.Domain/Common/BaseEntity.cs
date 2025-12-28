namespace Academic.Domain.Common
{
    // All domain entities will inherit from this
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Primary Key

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}