using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Gerenciador.Usuarios.Api.Application.Responses;
using ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.AggregateModels.UsuarioModel;
using ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Gerenciador.Usuarios.Api.Application.Handlers
{
    public class RegistrarNovoUsuarioHandler : IRequestHandler<RegistrarNovoUsuarioCommand, Result<RegistrarNovoUsuarioResponse>>
    {
        public Task<Result<RegistrarNovoUsuarioResponse>> Handle(RegistrarNovoUsuarioCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}