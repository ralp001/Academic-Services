using Academic.Application.Interfaces;
using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.Subjects.Commands
{
    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, Guid>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAppLogger<CreateSubjectCommandHandler> _logger; // Use the injected logger

        public CreateSubjectCommandHandler(ISubjectRepository subjectRepository, IAppLogger<CreateSubjectCommandHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            // 1. Convert Command DTO to Domain Entity
            var newSubject = new Subject
            {
                Name = request.Name,
                Description = request.Description,
                LeadTeacherId = request.LeadTeacherId
            };

            // 2. Persist the Entity
            await _subjectRepository.AddAsync(newSubject);

            // 3. Log the successful creation (Business Event)
            _logger.LogInformation("Subject created successfully: {SubjectName} (ID: {SubjectId})",
                newSubject.Name, newSubject.Id);

            // 4. Return the ID
            return newSubject.Id;
        }
    }
}