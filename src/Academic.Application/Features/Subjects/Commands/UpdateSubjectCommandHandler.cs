using Academic.Application.Exceptions;
using Academic.Application.Interfaces;
using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.Subjects.Commands
{
    public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, Unit>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAppLogger<UpdateSubjectCommandHandler> _logger;

        public UpdateSubjectCommandHandler(ISubjectRepository subjectRepository, IAppLogger<UpdateSubjectCommandHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the existing Domain Entity by ID
            var subjectToUpdate = await _subjectRepository.GetByIdAsync(request.Id);

            if (subjectToUpdate == null)
            {
                _logger.LogWarning("Update failed. Subject not found for ID: {SubjectId}", request.Id);
                // Throw an exception for the API controller to handle (e.g., return HTTP 404)
                throw new NotFoundException(nameof(Class), request.Id);
            }

            // 2. Update the Entity's Properties from the command
            subjectToUpdate.Name = request.Name;
            subjectToUpdate.Description = request.Description;
            subjectToUpdate.LeadTeacherId = request.LeadTeacherId;

            // 3. Persist the changes using the repository
            await _subjectRepository.UpdateAsync(subjectToUpdate);

            _logger.LogInformation("Subject updated successfully: {SubjectName} (ID: {SubjectId})",
                subjectToUpdate.Name, subjectToUpdate.Id);

            return Unit.Value;
        }
    }
}