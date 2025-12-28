using Academic.Application.Features.Classes.Commands;
using Academic.Domain.Interfaces;
using FluentValidation;
using System.Threading.Tasks;

namespace Academic.Application.Features.Classes.Commands
{
    public class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
    {
        private readonly IClassRepository _classRepository;

        // Inject the repository to check for business rules (e.g., uniqueness)
        public CreateClassCommandValidator(IClassRepository classRepository)
        {
            _classRepository = classRepository;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Class Name is required.")
                .MaximumLength(50).WithMessage("Class Name cannot exceed 50 characters.")
                .MustAsync(BeUniqueClassName).WithMessage("A class with this name already exists.");

            RuleFor(c => c.Level)
                .IsInEnum().WithMessage("A valid academic level (JSS1-SSS3) is required.");

            RuleFor(c => c.MaxCapacity)
                .GreaterThan(10).WithMessage("Class capacity must be greater than 10.");

            RuleFor(c => c.Stream)
                .MaximumLength(20).WithMessage("Stream name cannot exceed 20 characters.");
        }

        // Custom validation method to check database uniqueness
        private async Task<bool> BeUniqueClassName(string name, CancellationToken cancellationToken)
        {
            // Use the specific repository method we defined!
            return await _classRepository.GetByNameAsync(name) == null;
        }
    }
}