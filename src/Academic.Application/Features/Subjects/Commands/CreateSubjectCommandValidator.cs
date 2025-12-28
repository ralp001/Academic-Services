using Academic.Domain.Interfaces;
using FluentValidation;

namespace Academic.Application.Features.Subjects.Commands
{
    public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
    {
        private readonly ISubjectRepository _subjectRepository;

        // Inject the repository to check the uniqueness rule
        public CreateSubjectCommandValidator(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;

            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Subject Name is required.")
                .MaximumLength(100).WithMessage("Subject Name cannot exceed 100 characters.")
                .MustAsync(BeUniqueSubjectName).WithMessage("A subject with this name already exists.");

            RuleFor(s => s.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }

        // Custom validation method to check database uniqueness
        private async Task<bool> BeUniqueSubjectName(string name, CancellationToken cancellationToken)
        {
            // Use the specific repository method we defined in ISubjectRepository
            return await _subjectRepository.GetByNameAsync(name) == null;
        }
    }
}