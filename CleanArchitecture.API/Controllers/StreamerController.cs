using CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer;
using CleanArchitecture.Application.Features.Streamers.Commands.DeleteStreamer;
using CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer;
using CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUrl;
using CleanArchitecture.Application.Features.Streamers.Queries.GetStreamerListByUsername;
using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StreamerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // StreamersVm is a dto (data transfer object) class, we use it to decouple the presentation layer(api)
        // of the core (domain) layer, also dto's define how the data will be sent over the network.
        [HttpGet("ByUsername/{username}", Name = "GetStreamerByUsername")]
        [ProducesResponseType(typeof(IEnumerable<StreamersVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<StreamersVm>>> GetStreamerByUsername(string username)
        {
            var query = new GetStreamerListQuery(username);
            var streamers = await _mediator.Send(query);
            return Ok(streamers);
        }

        [HttpGet("ByUrl/{url}", Name = "GetStreamerByUrl")]
        [ProducesResponseType(typeof(IEnumerable<StreamersVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<StreamersVm>>> GetStreamerByUrl(string url)
        {
            var query = new GetStreamerListByUrlQuery(url);
            var streamers = await _mediator.Send(query);
            return Ok(streamers);
        }

        [HttpPost(Name = "CreateStreamer")]
        // by adding this we state that only authorized Administrador users can access this endpoint
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateStreamer([FromBody] CreateStreamerCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(Name = "UpdateStreamer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateStreamer([FromBody] UpdateStreamerCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}",Name = "DeleteStreamer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteStreamer(int id)
        {
            var command = new DeleteStreamerCommand() { Id = id};

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
