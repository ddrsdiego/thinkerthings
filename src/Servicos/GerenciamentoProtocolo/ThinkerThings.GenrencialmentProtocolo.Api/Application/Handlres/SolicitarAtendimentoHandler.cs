using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Validators;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Handlres
{
    public class SolicitarAtendimentoHandler : IRequestHandler<SolicitarAtendimentoCommand, Result<SolicitarAtendimentoResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IProtocoloServico _protocoloServico;
        private readonly IUsuarioSolicitanteServico _usuarioSolicitanteServico;

        public SolicitarAtendimentoHandler(IMediator mediator,
                                           IProtocoloServico protocoloServico,
                                           IUsuarioSolicitanteServico usuarioSolicitanteServico)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _protocoloServico = protocoloServico ?? throw new ArgumentNullException(nameof(protocoloServico));
            _usuarioSolicitanteServico = usuarioSolicitanteServico ?? throw new ArgumentNullException(nameof(usuarioSolicitanteServico));
        }

        public async Task<Result<SolicitarAtendimentoResponse>> Handle(SolicitarAtendimentoCommand request, CancellationToken cancellationToken)
        {
            var validarRequestResult = ValidarRequest(request);
            if (validarRequestResult.IsFailure)
                return Result<SolicitarAtendimentoResponse>.Fail(validarRequestResult.Messages);

            var usuarioResult = await ConsultarUsuarioSolicitante(request);
            if (usuarioResult.IsFailure)
                return Result<SolicitarAtendimentoResponse>.Fail(usuarioResult.Messages);

            var gerarNumeroProtocoloResult = await GerarNumeroProtocolo().ConfigureAwait(false);
            if (gerarNumeroProtocoloResult.IsFailure)
                return Result<SolicitarAtendimentoResponse>.Fail(gerarNumeroProtocoloResult.Messages);

            var novoProtocolo = new Protocolo
            {
                NumeroProtocolo = gerarNumeroProtocoloResult.Value,
                SolicitanteProtocolo = new ProtocoloUsuarioSolicitante
                {
                    NumeroDocumento = usuarioResult.Value.NumeroDocumento,
                    EmailSolicitante = usuarioResult.Value.EmailSolicitante,
                }
            };

            novoProtocolo.AdicionarDetalhe(new ProtocoloDetalhe(ProtocoloDetalheItem.Solicitado));
            await _protocoloServico.SolicitarProtocolo(novoProtocolo).ConfigureAwait(false);

            return Result<SolicitarAtendimentoResponse>.Ok(new SolicitarAtendimentoResponse(novoProtocolo.NumeroProtocolo, novoProtocolo.DataSolicitacao));
        }

        private async Task<Result<UsuarioSolicitante>> ConsultarUsuarioSolicitante(SolicitarAtendimentoCommand request)
        {
            try
            {
                var usuarioResult = await _usuarioSolicitanteServico.ConsultarUsuarioSolicitante(request.NumeroDocumento, request.EmailSolicitante);
                if (usuarioResult.IsSuccess)
                    return Result<UsuarioSolicitante>.Ok(usuarioResult.Value);

                var response = await _mediator.Send(new RegistrarNovoUsuarioSolicitanteCommand(request.NumeroDocumento, request.EmailSolicitante, request.NomeSolicitante, request.TelefoneSolicitante));
                if (response.IsFailure)
                    return Result<UsuarioSolicitante>.Fail(response.Messages);

                return Result<UsuarioSolicitante>.Ok(new UsuarioSolicitante { NumeroDocumento = response.Value.NumeroDocumento, EmailSolicitante = response.Value.EmailSolicitante });
            }
            catch (Exception ex)
            {
                return Result<UsuarioSolicitante>.Fail("");
            }
        }

        private static Result ValidarRequest(SolicitarAtendimentoCommand request)
        {
            var validator = new SolicitarAtendimentoCommandValidator();

            var result = validator.Validate(request);
            if (!result.IsValid)
                return Result.Fail(result.Errors.Select(x => x.ErrorMessage));

            return Result.Ok();
        }

        private async Task<Result<string>> GerarNumeroProtocolo()
        {
            try
            {
                var numeroProtocolo = await _protocoloServico.GerarNumeroProtocolo().ConfigureAwait(false);
                if (numeroProtocolo.IsFailure)
                    return Result<string>.Fail(numeroProtocolo.Messages);

                return Result<string>.Ok(numeroProtocolo.Value);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("");
            }
        }
    }
}