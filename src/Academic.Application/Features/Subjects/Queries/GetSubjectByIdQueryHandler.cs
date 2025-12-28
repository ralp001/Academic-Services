using Academic.Application.Exceptions;
using Academic.Application.Interfaces;
using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.Subjects.Queries
{
    public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAppLogger<GetSubjectByIdQueryHandler> _logger;

        public GetSubjectByIdQueryHandler(ISubjectRepository subjectRepository,
                                          IAppLogger<GetSubjectByIdQueryHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _logger = logger;
        }

        public async Task<SubjectDto> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve subject with ID: {SubjectId}", request.Id);

            // 1. Fetch the Domain Entity
            var subjectEntity = await _subjectRepository.GetByIdAsync(request.Id);

            if (subjectEntity == null)
            {
                _logger.LogWarning("Subject not found for ID: {SubjectId}", request.Id);
                // Throwing an ApplicationException for the controller to catch as a 404
                throw new NotFoundException(nameof(Class), request.Id);
            }

            // 2. Map Domain Entity to DTO
            var subjectDto = new SubjectDto
            {
                Id = subjectEntity.Id,
                Name = subjectEntity.Name,
                Description = subjectEntity.Description,
                LeadTeacherId = subjectEntity.LeadTeacherId
            };

            _logger.LogInformation("Successfully retrieved subject: {SubjectName}", subjectEntity.Name);

            // 3. Return the mapped DTO
            return subjectDto;
        }
    }
}