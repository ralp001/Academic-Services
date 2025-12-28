using Academic.Domain.Interfaces;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.ClassSubjects.Commands
{
    public class AssignSubjectToClassCommandValidator : AbstractValidator<AssignSubjectToClassCommand>
    {
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassSubjectRepository _classSubjectRepository; // We need a new repository interface here

        public AssignSubjectToClassCommandValidator(IClassRepository classRepository,
                                                    ISubjectRepository subjectRepository,
                                                    IClassSubjectRepository classSubjectRepository)
        {
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _classSubjectRepository = classSubjectRepository;

            RuleFor(c => c.ClassId)
                .NotEmpty().WithMessage("Class ID is required.")
                .MustAsync(ClassMustExist).WithMessage("The specified Class does not exist.");

            RuleFor(c => c.SubjectId)
                .NotEmpty().WithMessage("Subject ID is required.")
                .MustAsync(SubjectMustExist).WithMessage("The specified Subject does not exist.");

            RuleFor(c => c) // Validating the whole command object for the unique constraint
                .MustAsync(AssignmentMustBeUnique).WithMessage("This subject is already assigned to this class.");

            RuleFor(c => c.WeeklyHours)
                .GreaterThan(0).WithMessage("Weekly hours must be a positive number.");
        }

        // --- Existence Checks ---
        private async Task<bool> ClassMustExist(Guid classId, CancellationToken cancellationToken)
        {
            return await _classRepository.GetByIdAsync(classId) != null;
        }

        private async Task<bool> SubjectMustExist(Guid subjectId, CancellationToken cancellationToken)
        {
            return await _subjectRepository.GetByIdAsync(subjectId) != null;
        }

        // --- Uniqueness Check (Crucial for M-to-M) ---
        private async Task<bool> AssignmentMustBeUnique(AssignSubjectToClassCommand command, CancellationToken cancellationToken)
        {
            // NOTE: This requires IClassSubjectRepository to have a method like IsAssignedAsync
            return !await _classSubjectRepository.IsAssignedAsync(command.ClassId, command.SubjectId);
        }
    }
}