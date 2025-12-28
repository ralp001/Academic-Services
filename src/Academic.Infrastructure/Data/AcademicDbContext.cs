using Microsoft.EntityFrameworkCore;
using Academic.Domain.Entities;
using Academic.Domain.Common; // For BaseEntity reference
using System.Reflection;

namespace Academic.Infrastructure.Data // Make sure this namespace is used
{
    // The DbContext needs to reference the Options and inherit from DbContext
    public class AcademicDbContext : DbContext
    {
        public AcademicDbContext(DbContextOptions<AcademicDbContext> options)
            : base(options)
        {
        }

        // --- Core DbSets (Tables) ---
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassSubject> ClassSubjects { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        // ... (other DbSets for Timetable, Assessment, Result will be added later) ...

        // --- Configuration for Data Model ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applies configurations from separate files, if defined
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClassSubject>()
                .HasKey(cs => new { cs.ClassId, cs.SubjectId }); // Define the composite PK

            // 2. Configure the Class -> ClassSubject relationship
            modelBuilder.Entity<ClassSubject>()
                .HasOne(cs => cs.Class) // One ClassSubject belongs to one Class
                .WithMany(c => c.ClassSubjects) // One Class has many ClassSubjects
                .HasForeignKey(cs => cs.ClassId) // Use ClassId as the FK
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete (safety)

            // 3. Configure the Subject -> ClassSubject relationship
            modelBuilder.Entity<ClassSubject>()
                .HasOne(cs => cs.Subject) // One ClassSubject belongs to one Subject
                .WithMany(s => s.ClassSubjects) // One Subject has many ClassSubjects
                .HasForeignKey(cs => cs.SubjectId) // Use SubjectId as the FK
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete (safety)

            // Configure Assessment relationships
            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.Class)
                .WithMany() // Assumes Class doesn't need a direct ICollection<Assessment>
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.Subject)
                .WithMany() // Assumes Subject doesn't need a direct ICollection<Assessment>
                .HasForeignKey(a => a.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Apply other entity configurations (like the index for Subject Name, etc.)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        

        // --- Auditing/Interceptor Logic (Updated for CreatedAt/UpdatedAt) ---
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}