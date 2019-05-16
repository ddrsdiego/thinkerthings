using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Handlres
{
    public class IniciarAtendimentoProtocoloHandler : IRequestHandler<IniciarAtendimentoProtocoloCommand, Result<IniciarAtendimentoProtocoloResponse>>
    {
        private readonly IProtocoloServico _protocoloServico;

        public IniciarAtendimentoProtocoloHandler(IProtocoloServico protocoloServico)
        {
            _protocoloServico = protocoloServico;
        }

        public async Task<Result<IniciarAtendimentoProtocoloResponse>> Handle(IniciarAtendimentoProtocoloCommand request, CancellationToken cancellationToken)
        {
            var protocoloResult = await _protocoloServico.ConsultarProtocoloPorNumero(request.NumeroProtocolo).ConfigureAwait(false);
            if (protocoloResult.IsFailure)
                return Result<IniciarAtendimentoProtocoloResponse>.Fail(protocoloResult.Messages);

            if (protocoloResult.Value.StatusProtocolo != StatusProtocolo.Solicitado)
                return Result<IniciarAtendimentoProtocoloResponse>.Fail($"{nameof(StatusProtocolo)} não esta no status de solicitado.");

            protocoloResult.Value.StatusProtocolo = StatusProtocolo.Inicializado;
            protocoloResult.Value.AdicionarDetalhe(new ProtocoloDetalhe(ProtocoloDetalheItem.EmAtendimento));

            var result = await _protocoloServico.Alterar(protocoloResult.Value).ConfigureAwait(false);
            if (result.IsFailure)
                return Result<IniciarAtendimentoProtocoloResponse>.Fail(result.Messages);

            return Result<IniciarAtendimentoProtocoloResponse>.Ok(new IniciarAtendimentoProtocoloResponse());
        }
    }
}