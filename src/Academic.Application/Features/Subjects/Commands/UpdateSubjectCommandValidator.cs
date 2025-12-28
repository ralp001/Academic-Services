using Academic.Domain.Interfaces;
using FluentValidation;
using System.Threading.Tasks;

namespace Academic.Application.Features.Subjects.Commands
{
    public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
    {
        private readonly ISubjectRepository _subjectRepository;

        public UpdateSubjectCommandValidator(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;

            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Subject ID is required for update.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Subject Name is required.")
                .MaximumLength(100).WithMessage("Subject Name cannot exceed 100 characters.")
                // Custom rule to check uniqueness against existing names, ignoring the current subject's ID
                .MustAsync(BeUniqueSubjectName).WithMessage("A different subject with this name already exists.");

            RuleFor(c => c.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }

        // Custom validation method: Checks if the new name conflicts with ANY *other* subject
        private async Task<bool> BeUniqueSubjectName(UpdateSubjectCommand command, string name, CancellationToken cancellationToken)
        {
            // Attempt to fetch a subject with the new name
            var existingSubject = await _subjectRepository.GetByNameAsync(name);

            // It is unique if:
            // 1. No subject exists with that name (existingSubject == null)
            // OR
            // 2. A subject exists, but its ID matches the ID of the subject we are currently updating.
            return existingSubject == null || existingSubject.Id == command.Id;
        }
    }
}