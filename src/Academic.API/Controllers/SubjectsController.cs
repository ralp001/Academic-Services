using Academic.Application.Features.Subjects.Commands; // Reference the new Command
using Academic.Application.Features.Subjects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization; // Conceptual security attribute

namespace Academic.API.Controllers
{
    [ApiController]
    [Route("api/v1/academic/subjects")] // Specific, versioned route for subject management
    [Produces(MediaTypeNames.Application.Json)]
    // [Authorize(Roles = "CurriculumManager, ITAdmin")] // Only authorized staff can access
    public class SubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Dependency Injection: Inject IMediator
        public SubjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost] // The endpoint will be POST /api/v1/academic/subjects
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For validation errors
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectCommand command)
        {
            // 1. Send the command through the MediatR pipeline.
            // This triggers the ValidationBehavior, LoggingBehavior, and then the CreateSubjectCommandHandler.
            var subjectId = await _mediator.Send(command);

            // 2. Return a 201 Created response.
            return CreatedAtAction(nameof(CreateSubject), new { id = subjectId }, subjectId);
        }

        [HttpGet] // The endpoint will be GET /api/v1/academic/subjects
        [ProducesResponseType(typeof(List<SubjectDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSubjects()
        {
            // 1. Create and send the query
            var query = new GetAllSubjectsQuery();

            // 2. IMediator sends the query and awaits the List<SubjectDto> response
            var result = await _mediator.Send(query);

            // 3. Return a 200 OK response with the list of subjects
            return Ok(result);
        }
        // --- Future endpoints like GetSubjectById, UpdateSubject will go here ---
        [HttpGet("{id}")] // The endpoint will be GET /api/v1/academic/subjects/{id}
        [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // For subject not found
        public async Task<IActionResult> GetSubjectById(Guid id)
        {
            
            {
                // 1. Create and send the query with the ID from the route
                var query = new GetSubjectByIdQuery(id);

                // 2. IMediator sends the query
                var result = await _mediator.Send(query);

                // 3. Return a 200 OK response with the subject details
                return Ok(result);
            }
            
        }
        [HttpPut("{id}")] // The endpoint will be PUT /api/v1/academic/subjects/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Standard response for successful update
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] UpdateSubjectCommand command)
        {
            // Ensure the ID in the route matches the ID in the body for safety
            if (id != command.Id)
            {
                return BadRequest("ID in the route does not match the ID in the request body.");
            }

            
            {
                // Send the command through the MediatR pipeline.
                await _mediator.Send(command);

                // Return 204 No Content, indicating successful update with no body to return.
                return NoContent();
            }
            
        }
        [HttpDelete("{id}")] // The endpoint will be DELETE /api/v1/academic/subjects/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Standard response for successful deletion
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            
            {
                // 1. Create and send the command with the ID from the route
                var command = new DeleteSubjectCommand(id);

                // 2. Send the command through the MediatR pipeline.
                await _mediator.Send(command);

                // Return 204 No Content, indicating successful deletion.
                return NoContent();
            }
            
        }
    }
}
