using MediatR;
using System;
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
            var validarRequestResult = ValidarRequest(request);
            if (validarRequestResult.IsFailure)
                return Result<RegistrarNovoUsuarioSolicitanteResponse>.Fail(validarRequestResult.Messages);

            var novoUsuarioSolicitante = new UsuarioSolicitante
            {
                EmailSolicitante = request.EmailSolicitante,
                NomeSolicitante = request.NomeSolicitante,
                TelefoneSolicitante = request.TelefoneSolicitante,
                CPFSolicitante = request.CPFSolicitante
            };

            var registrarUsuarioSolicitanteResult = await RegistrarUsuarioSolicitante(novoUsuarioSolicitante);
            if (registrarUsuarioSolicitanteResult.IsFailure)
                return Result<RegistrarNovoUsuarioSolicitanteResponse>.Fail(registrarUsuarioSolicitanteResult.Messages);

            return Result<RegistrarNovoUsuarioSolicitanteResponse>.Ok(CreateResponse(novoUsuarioSolicitante));
        }

        private static Result ValidarRequest(RegistrarNovoUsuarioSolicitanteCommand request)
        {
            var validator = new RegistrarNovoUsuarioSolicitanteCommandValidator();
            var validateResult = validator.Validate(request);
            if (!validateResult.IsValid)
                return Result.Fail(validateResult.Errors.Select(x => x.ErrorMessage));

            return Result.Ok();
        }

        private async Task<Result> RegistrarUsuarioSolicitante(UsuarioSolicitante novoUsuarioSolicitante)
        {
            try
            {
                var registrarUsuarioSolicitanteResult = await _usuarioSolicitanteServico.RegistrarUsuarioSolicitante(novoUsuarioSolicitante);
                if (registrarUsuarioSolicitanteResult.IsFailure)
                    return Result.Fail(registrarUsuarioSolicitanteResult.Messages);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("");
            }
        }

        private static RegistrarNovoUsuarioSolicitanteResponse CreateResponse(UsuarioSolicitante novoUsuarioSolicitante)
        {
            return new RegistrarNovoUsuarioSolicitanteResponse
            {
                CPFSolicitante = novoUsuarioSolicitante.CPFSolicitante,
                EmailSolicitante = novoUsuarioSolicitante.EmailSolicitante,
            };
        }
    }
}