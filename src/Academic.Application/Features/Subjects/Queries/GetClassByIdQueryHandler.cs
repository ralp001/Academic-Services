using Academic.Application.Exceptions;
using Academic.Application.Interfaces;
using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.Classes.Queries
{
    public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, ClassDto>
    {
        private readonly IClassRepository _classRepository;
        private readonly IAppLogger<GetClassByIdQueryHandler> _logger;

        public GetClassByIdQueryHandler(IClassRepository classRepository,
                                        IAppLogger<GetClassByIdQueryHandler> logger)
        {
            _classRepository = classRepository;
            _logger = logger;
        }

        public async Task<ClassDto> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve class with ID: {ClassId}", request.Id);

            // 1. Fetch the Domain Entity from the Infrastructure layer
            var classEntity = await _classRepository.GetByIdAsync(request.Id);

            if (classEntity == null)
            {
                _logger.LogWarning("Class not found for ID: {ClassId}", request.Id);
                // In a real application, you might throw a custom NotFoundException 
                // that a global middleware catches and turns into an HTTP 404.
                throw new NotFoundException(nameof(Class), request.Id);
            }

            // 2. Map Domain Entity to DTO
            var classDto = new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                // Conversion from Enum to String
                Level = classEntity.Level.ToString(),
                Stream = classEntity.Stream,
                MaxCapacity = classEntity.MaxCapacity,
                FormTeacherId = classEntity.FormTeacherId
            };

            _logger.LogInformation("Successfully retrieved class: {ClassName}", classEntity.Name);

            // 3. Return the mapped DTO
            return classDto;
        }
    }
}