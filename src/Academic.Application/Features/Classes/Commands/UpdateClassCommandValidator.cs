using Academic.Domain.Interfaces;
using FluentValidation;
using System.Threading.Tasks;
using System.Linq;

namespace Academic.Application.Features.Classes.Commands
{
    public class UpdateClassCommandValidator : AbstractValidator<UpdateClassCommand>
    {
        private readonly IClassRepository _classRepository;

        public UpdateClassCommandValidator(IClassRepository classRepository)
        {
            _classRepository = classRepository;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Class ID is required for update.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Class Name is required.")
                .MaximumLength(50).WithMessage("Class Name cannot exceed 50 characters.")
                // Custom rule to check uniqueness against existing names
                .MustAsync(BeUniqueClassName).WithMessage("A different class with this name already exists.");

            RuleFor(c => c.Level)
                .IsInEnum().WithMessage("A valid academic level (JSS1-SSS3) is required.");

            RuleFor(c => c.MaxCapacity)
                .GreaterThan(10).WithMessage("Class capacity must be greater than 10.");
        }

        // Custom validation method: Checks if the new name conflicts with ANY *other* class
        private async Task<bool> BeUniqueClassName(UpdateClassCommand command, string name, CancellationToken cancellationToken)
        {
            // Fetch all classes with the new name
            var existingClass = await _classRepository.GetByNameAsync(name);

            // It is unique if:
            // 1. No class exists with that name (existingClass == null)
            // OR
            // 2. A class exists, but its ID matches the ID of the class we are currently updating.
            return existingClass == null || existingClass.Id == command.Id;
        }
    }
}