using Academic.Application.Exceptions;
using Academic.Application.Interfaces;
using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;

namespace Academic.Application.Features.Classes.Commands
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, Unit>
    {
        private readonly IClassRepository _classRepository;
        private readonly IAppLogger<UpdateClassCommandHandler> _logger;

        public UpdateClassCommandHandler(IClassRepository classRepository, IAppLogger<UpdateClassCommandHandler> logger)
        {
            _classRepository = classRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the existing Domain Entity by ID
            var classToUpdate = await _classRepository.GetByIdAsync(request.Id);

            if (classToUpdate == null)
            {
                _logger.LogWarning("Update failed. Class not found for ID: {ClassId}", request.Id);
                // Throw an exception for the API controller to handle (e.g., return HTTP 404)
                throw new NotFoundException(nameof(Class), request.Id);
            }

            // 2. Update the Entity's Properties from the command
            classToUpdate.Name = request.Name;
            classToUpdate.Level = request.Level;
            classToUpdate.Stream = request.Stream;
            classToUpdate.MaxCapacity = request.MaxCapacity;
            classToUpdate.FormTeacherId = request.FormTeacherId;

            // Note: The UpdatedAt timestamp is automatically set by the DbContext auditing logic!

            // 3. Persist the changes using the repository
            await _classRepository.UpdateAsync(classToUpdate);

            _logger.LogInformation("Class updated successfully: {ClassName} (ID: {ClassId})",
                classToUpdate.Name, classToUpdate.Id);

            // MediatR 'Unit' means the operation succeeded but returns no data.
            return Unit.Value;
        }
    }
}