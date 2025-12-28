using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;
using Academic.Application.Interfaces; // <-- NEW
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Features.Classes.Commands
{
    public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Guid>
    {
        private readonly IClassRepository _classRepository;
        private readonly IAppLogger<CreateClassCommandHandler> _logger; // <-- 1. DECLARE LOGGER

        // Add IAppLogger to the constructor for Dependency Injection
        public CreateClassCommandHandler(IClassRepository classRepository,
                                         IAppLogger<CreateClassCommandHandler> logger) // <-- 2. INJECT LOGGER
        {
            _classRepository = classRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            // 1. Convert Command DTO to Domain Entity
            var newClass = new Class
            {
                Name = request.Name,
                Level = request.Level,
                Stream = request.Stream,
                MaxCapacity = request.MaxCapacity,
                FormTeacherId = request.FormTeacherId
            };

            // 2. Persist the Entity
            // The GenericRepository base class handles the SaveChangesAsync call.
            await _classRepository.AddAsync(newClass);

            // 3. Log the successful creation event
            _logger.LogInformation("Class created successfully: {ClassName} (ID: {ClassId}, Level: {Level})",
                newClass.Name, newClass.Id, newClass.Level); // <-- 3. USE LOGGER

            // 4. Return the ID of the newly created entity
            return newClass.Id;
        }
    }
}