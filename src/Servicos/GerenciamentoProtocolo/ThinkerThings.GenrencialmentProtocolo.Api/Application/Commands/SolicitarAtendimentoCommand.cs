using MediatR;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands
{
    public class SolicitarAtendimentoCommand : IRequest<Result<SolicitarAtendimentoResponse>>
    {
        public SolicitarAtendimentoCommand(string nomeSolicitante, string emailSolicitante, string telefoneSolicitante, string numeroDocumento)
        {
            NomeSolicitante = nomeSolicitante;
            EmailSolicitante = emailSolicitante;
            TelefoneSolicitante = telefoneSolicitante;
            NumeroDocumento = numeroDocumento;
        }

        public string NomeSolicitante { get; }
        public string EmailSolicitante { get; }
        public string TelefoneSolicitante { get; }
        public string NumeroDocumento { get; }
    }
}
