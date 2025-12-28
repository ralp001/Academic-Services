using Academic.Application.Interfaces;
using Academic.Domain.Interfaces;
using MediatR;

namespace Academic.Application.Features.Classes.Queries
{
    // The handler implements IRequestHandler<Query, Response>
    public class GetAllClassesQueryHandler : IRequestHandler<GetAllClassesQuery, List<ClassDto>>
    {
        private readonly IClassRepository _classRepository;
        private readonly IAppLogger<GetAllClassesQueryHandler> _logger;

        public GetAllClassesQueryHandler(IClassRepository classRepository,
                                         IAppLogger<GetAllClassesQueryHandler> logger)
        {
            _classRepository = classRepository;
            _logger = logger;
        }

        public async Task<List<ClassDto>> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all classes from the database.");

            // 1. Fetch the list of Domain Entities from the Infrastructure layer
            var classes = await _classRepository.GetAllAsync();

            if (classes == null || !classes.Any())
            {
                _logger.LogWarning("No academic classes found in the system.");
                return new List<ClassDto>();
            }

            // 2. Map Domain Entities to DTOs (Data Transfer Objects)
            var classDtos = classes
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    Name = c.Name,

                    // --- KEY CONVERSION: Enum to String for Display ---
                    Level = c.Level.ToString(),
                    // --------------------------------------------------

                    Stream = c.Stream,
                    MaxCapacity = c.MaxCapacity,
                    FormTeacherId = c.FormTeacherId
                })
                .ToList();

            _logger.LogInformation("Successfully retrieved {Count} classes.", classDtos.Count);

            // 3. Return the mapped DTOs
            return classDtos;
        }
    }
}