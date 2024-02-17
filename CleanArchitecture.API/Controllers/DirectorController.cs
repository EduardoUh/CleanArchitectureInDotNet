using CleanArchitecture.Application.Features.Directors.Commands.CreateDirector;
using CleanArchitecture.Application.Features.Directors.Queries.PaginationDirector;
using CleanArchitecture.Application.Features.Directors.Queries.Vms;
using CleanArchitecture.Application.Features.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DirectorController(IMediator mediator) : ControllerBase
    {
        private IMediator _mediator = mediator;

        [HttpPost(Name = "CreateDirector")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateDirector([FromBody] CreateDirectorCommand command)
        {
            // you might think that we are no implementing unit of work here, but are using it in the application layer
            // in the CQRS pattern
            return await _mediator.Send(command);
        }

        [HttpGet("pagination", Name = "PaginationDirector")]
        [ProducesResponseType(typeof(PaginationVm<DirectorVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<DirectorVm>>> GetPaginationDirector(
            [FromQuery] PaginationDirectorsQuery paginationDirectorQuery
            )
        {
            var paginationDirector = await _mediator.Send(paginationDirectorQuery);

            return Ok(paginationDirector);
        }

    }
}
