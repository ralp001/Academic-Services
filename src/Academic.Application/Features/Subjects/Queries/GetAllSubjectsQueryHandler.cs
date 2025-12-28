using Academic.Domain.Interfaces;
using MediatR;
using Academic.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.Subjects.Queries
{
    public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, List<SubjectDto>>
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAppLogger<GetAllSubjectsQueryHandler> _logger;

        public GetAllSubjectsQueryHandler(ISubjectRepository subjectRepository,
                                          IAppLogger<GetAllSubjectsQueryHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _logger = logger;
        }

        public async Task<List<SubjectDto>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all subjects from the database.");

            // 1. Fetch the list of Domain Entities
            var subjects = await _subjectRepository.GetAllAsync();

            if (subjects == null || !subjects.Any())
            {
                _logger.LogWarning("No academic subjects found in the system.");
                return new List<SubjectDto>();
            }

            // 2. Map Domain Entities to DTOs
            var subjectDtos = subjects
                .Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    LeadTeacherId = s.LeadTeacherId
                })
                .ToList();

            _logger.LogInformation("Successfully retrieved {Count} subjects.", subjectDtos.Count);

            // 3. Return the mapped DTOs
            return subjectDtos;
        }
    }
}