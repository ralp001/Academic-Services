using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Academic.Application.Features.ClassSubjects.Commands;

namespace Academic.API.Controllers
{
    [ApiController]
    [Route("api/v1/academic/class-subjects")] // A resource for the assignments themselves
    [Produces(MediaTypeNames.Application.Json)]
    public class ClassSubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassSubjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignSubjectToClass([FromBody] AssignSubjectToClassCommand command)
        {
            // MediatR will handle validation and execution
            await _mediator.Send(command);

            // Return 201 Created, often returning the created entity or just a success status.
            // Since this returns Unit, 201 is appropriate for a successful resource creation.
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}