using Academic.Domain.Interfaces;
using MediatR;
using Academic.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.Subjects.Queries
{
    public class GetSubjectsByClassIdQueryHandler : IRequestHandler<GetSubjectsByClassIdQuery, List<SubjectDto>>
    {
        // Inject the repository that now holds the complex query
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAppLogger<GetSubjectsByClassIdQueryHandler> _logger;

        public GetSubjectsByClassIdQueryHandler(ISubjectRepository subjectRepository,
                                                IAppLogger<GetSubjectsByClassIdQueryHandler> logger)
        {
            _subjectRepository = subjectRepository;
            _logger = logger;
        }

        public async Task<List<SubjectDto>> Handle(GetSubjectsByClassIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve subjects for Class ID: {ClassId}", request.ClassId);

            // 1. Fetch the Subject Entities using the new custom repository method
            var assignedSubjects = await _subjectRepository.GetSubjectsByClassIdAsync(request.ClassId);

            if (assignedSubjects == null || !assignedSubjects.Any())
            {
                _logger.LogWarning("No subjects found for Class ID: {ClassId}", request.ClassId);
                return new List<SubjectDto>();
            }

            // 2. Map the Subject Entities to a list of Subject DTOs
            var subjectDtos = assignedSubjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                LeadTeacherId = s.LeadTeacherId
            }).ToList();

            _logger.LogInformation("Successfully retrieved {Count} subjects for class.", subjectDtos.Count);

            return subjectDtos;
        }
    }
}