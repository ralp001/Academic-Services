using Academic.Application.Exceptions;
using Academic.Application.Interfaces;
using Academic.Domain.Entities;
using Academic.Domain.Interfaces;
using MediatR;

namespace Academic.Application.Features.Classes.Commands
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, Unit>
    {
        private readonly IClassRepository _classRepository;
        private readonly IAppLogger<DeleteClassCommandHandler> _logger;

        public DeleteClassCommandHandler(IClassRepository classRepository, IAppLogger<DeleteClassCommandHandler> logger)
        {
            _classRepository = classRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the existing Domain Entity by ID
            var classToDelete = await _classRepository.GetByIdAsync(request.Id);

            if (classToDelete == null)
            {
                _logger.LogWarning("Deletion failed. Class not found for ID: {ClassId}", request.Id);
                throw new NotFoundException(nameof(Class), request.Id);
            }



            await _classRepository.DeleteAsync(classToDelete);
            // --- END SOFT DELETE LOGIC ---

            _logger.LogInformation("Class successfully deleted/archived (ID: {ClassId})", request.Id);

            return Unit.Value;
        }
    }
}