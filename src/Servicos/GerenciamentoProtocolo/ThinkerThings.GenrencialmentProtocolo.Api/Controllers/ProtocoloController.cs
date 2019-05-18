using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Controllers
{
    [ApiController]
    [Route("protocolo")]
    [Produces("application/json")]
    public class ProtocoloController : Controller
    {
        private readonly IMediator _mediator;

        public ProtocoloController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SolicitarAtendimento([FromBody] SolicitarAtendimentoCommand command)
        {
            if (command == null)
                return BadRequest();

            var response = await _mediator.Send(command).ConfigureAwait(false);
            if (response.IsFailure)
                return BadRequest(response.Messages.Select(x => x).ToArray());

            return Created("", response.Value);
        }
    }
}
