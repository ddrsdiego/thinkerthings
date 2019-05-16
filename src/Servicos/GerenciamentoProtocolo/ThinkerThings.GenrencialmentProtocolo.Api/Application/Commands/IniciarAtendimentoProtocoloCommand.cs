using MediatR;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands
{
    public class IniciarAtendimentoProtocoloCommand : IRequest<Result<IniciarAtendimentoProtocoloResponse>>
    {
        public IniciarAtendimentoProtocoloCommand(string numeroProtocolo)
        {
            NumeroProtocolo = numeroProtocolo;
        }

        public string NumeroProtocolo { get; }
    }
}
