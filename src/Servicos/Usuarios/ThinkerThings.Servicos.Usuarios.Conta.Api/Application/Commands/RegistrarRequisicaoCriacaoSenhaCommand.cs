using MediatR;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Responses;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands
{
    public class RegistrarRequisicaoCriacaoSenhaCommand : IRequest<Result<RegistrarRequisicaoCriacaoSenhaResponse>>
    {
        public RegistrarRequisicaoCriacaoSenhaCommand(int usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public int UsuarioId { get; }
    }
}
