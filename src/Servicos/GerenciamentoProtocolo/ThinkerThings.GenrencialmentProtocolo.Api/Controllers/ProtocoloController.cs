using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Controllers
{
    [ApiController]
    public class ProtocoloController : Controller
    {
        private readonly IMediator _mediator;

        public ProtocoloController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> IniciarAtendimento([FromBody] IniciarAtendimentoProtocoloCommand command)
        {
            if (command == null)
                return BadRequest();

            var response = await _mediator.Send(command).ConfigureAwait(false);
            if (response.IsFailure)
                return BadRequest(response.Messages.Select(x => x));

            return Created("", response.Value);
        }
    }
}
