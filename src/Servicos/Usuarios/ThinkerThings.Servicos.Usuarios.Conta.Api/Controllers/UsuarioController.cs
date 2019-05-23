using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : Controller
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}