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
    public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, Unit>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAppLogger<DeleteSubjectCommandHandler> _logger;

        public DeleteSubjectCommandHandler(ISubjectRepository subjectRepository, IAppLogger<DeleteSubjectCommandHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the existing Domain Entity by ID
            var subjectToDelete = await _subjectRepository.GetByIdAsync(request.Id);

            if (subjectToDelete == null)
            {
                _logger.LogWarning("Deletion failed. Subject not found for ID: {SubjectId}", request.Id);
                throw new NotFoundException(nameof(Class), request.Id);
            }

            // 2. Call the repository's delete method (responsible for soft deleting or archiving)
            await _subjectRepository.DeleteAsync(subjectToDelete);

            _logger.LogInformation("Subject successfully deleted/archived (ID: {SubjectId})", request.Id);

            return Unit.Value;
        }
    }
}