using Academic.Application.Features.Classes.Commands;
using Academic.Application.Features.Classes.Queries;
using Academic.Application.Features.Subjects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Academic.API.Controllers
{
    [ApiController]
    [Route("api/v1/academic/classes")] // Specific route for class management
    [Produces(MediaTypeNames.Application.Json)]
    // [Authorize(Roles = "AdmissionsOfficer, ITAdmin")] // Only authorized staff can access
    public class ClassesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost] // The endpoint will be POST /api/v1/academic/classes
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For validation errors
        public async Task<IActionResult> CreateClass([FromBody] CreateClassCommand command)
        {
            // 1. Send the command through the MediatR pipeline.
            // This triggers the ValidationBehavior and then the CreateClassCommandHandler.
            var classId = await _mediator.Send(command);

            // 2. Return a 201 Created response, which is the standard response 
            // for successful resource creation, along with the ID.
            return CreatedAtAction(nameof(CreateClass), new { id = classId }, classId);
        }

        // --- Future endpoints like GetClassById, GetClassList will go here ---
        [HttpGet] // The endpoint will be GET /api/v1/academic/classes
        [ProducesResponseType(typeof(List<ClassDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllClasses()
        {
            // 1. Create and send the query
            var query = new GetAllClassesQuery();

            // 2. IMediator sends the query and awaits the List<ClassDto> response
            var result = await _mediator.Send(query);

            // 3. Return a 200 OK response with the list of classes
            return Ok(result);
        }
        [HttpGet("{id}")] // The endpoint will be GET /api/v1/academic/classes/{id}
        [ProducesResponseType(typeof(ClassDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // For class not found
        public async Task<IActionResult> GetClassById(Guid id)
        {

            // 1. Create and send the query with the ID from the route
            var query = new GetClassByIdQuery(id);

            // 2. IMediator sends the query
            var result = await _mediator.Send(query);

            // 3. Return a 200 OK response with the class details
            return Ok(result);

        }


        [HttpPut("{id}")] // The endpoint will be PUT /api/v1/academic/classes/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Standard response for successful update
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateClass(Guid id, [FromBody] UpdateClassCommand command)
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
        [HttpDelete("{id}")] // The endpoint will be DELETE /api/v1/academic/classes/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Standard response for successful deletion
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
           
            {
                // 1. Create and send the command with the ID from the route
                var command = new DeleteClassCommand(id);

                // 2. Send the command through the MediatR pipeline.
                await _mediator.Send(command);

                // Return 204 No Content, indicating successful deletion.
                return NoContent();
            }
            
        }
        [HttpGet("{id}/subjects")] // The endpoint will be GET /api/v1/academic/classes/{id}/subjects
        [ProducesResponseType(typeof(List<SubjectDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Only if the class itself doesn't exist
        public async Task<IActionResult> GetSubjectsByClassId(Guid id)
        {
            // Note: We should ideally validate the ClassId exists here, but we rely on other 
            // queries (like GetClassByIdQuery) or a dedicated validator for that. 
            // Since this query returns an empty list if no subjects are found, we just run the query.

            var query = new GetSubjectsByClassIdQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }

}