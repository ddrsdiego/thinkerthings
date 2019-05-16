using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Validators;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Handlres
{
    public class RegistrarNovoUsuarioSolicitanteHandler : IRequestHandler<RegistrarNovoUsuarioSolicitanteCommand, Result<RegistrarNovoUsuarioSolicitanteResponse>>
    {
        private readonly IUsuarioSolicitanteServico _usuarioSolicitanteServico;

        public RegistrarNovoUsuarioSolicitanteHandler(IUsuarioSolicitanteServico usuarioSolicitanteServico)
        {
            _usuarioSolicitanteServico = usuarioSolicitanteServico;
        }

        public async Task<Result<RegistrarNovoUsuarioSolicitanteResponse>> Handle(RegistrarNovoUsuarioSolicitanteCommand request, CancellationToken cancellationToken)
        {
            var validator = new RegistrarNovoUsuarioSolicitanteCommandValidator();
            var validateResult = validator.Validate(request);
            if (!validateResult.IsValid)
                return Result<RegistrarNovoUsuarioSolicitanteResponse>.Fail(validateResult.Errors.Select(x => x.ErrorMessage));

            var novoUsuarioSolicitante = new UsuarioSolicitante
            {
                NumeroDocumento = request.NumeroDocumento,
                EmailSolicitante = request.EmailSolicitante,
                NomeSolicitante = request.NomeSolicitante,
                TelefoneSolicitante = request.TelefoneSolicitante
            };

            var registrarUsuarioSolicitanteResult = await _usuarioSolicitanteServico.RegistrarUsuarioSolicitante(novoUsuarioSolicitante);
            if (registrarUsuarioSolicitanteResult.IsFailure)
                return Result<RegistrarNovoUsuarioSolicitanteResponse>.Fail(registrarUsuarioSolicitanteResult.Messages);

            return Result<RegistrarNovoUsuarioSolicitanteResponse>.Ok(CreateResponse(novoUsuarioSolicitante));
        }

        private static RegistrarNovoUsuarioSolicitanteResponse CreateResponse(UsuarioSolicitante novoUsuarioSolicitante)
        {
            return new RegistrarNovoUsuarioSolicitanteResponse
            {
                EmailSolicitante = novoUsuarioSolicitante.EmailSolicitante,
                NumeroDocumento = novoUsuarioSolicitante.NumeroDocumento
            };
        }
    }
}