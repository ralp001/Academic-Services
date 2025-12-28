using Academic.Application.Interfaces;
using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;

namespace Academic.Application.Features.ClassSubjects.Commands
{
    public class AssignSubjectToClassCommandHandler : IRequestHandler<AssignSubjectToClassCommand, Unit>
    {
        private readonly IClassSubjectRepository _classSubjectRepository;
        private readonly IAppLogger<AssignSubjectToClassCommandHandler> _logger;

        public AssignSubjectToClassCommandHandler(IClassSubjectRepository classSubjectRepository,
                                                  IAppLogger<AssignSubjectToClassCommandHandler> logger)
        {
            _classSubjectRepository = classSubjectRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(AssignSubjectToClassCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to assign Subject {SubjectId} to Class {ClassId}",
                request.SubjectId, request.ClassId);

            // 1. Create the new join entity
            var classSubject = new ClassSubject
            {
                ClassId = request.ClassId,
                SubjectId = request.SubjectId,
                IsCompulsory = request.IsCompulsory,
                WeeklyHours = request.WeeklyHours
            };

            // 2. Add and save the new entity
            await _classSubjectRepository.AddAsync(classSubject);

            _logger.LogInformation("Subject successfully assigned to class.");

            return Unit.Value;
        }
    }
}