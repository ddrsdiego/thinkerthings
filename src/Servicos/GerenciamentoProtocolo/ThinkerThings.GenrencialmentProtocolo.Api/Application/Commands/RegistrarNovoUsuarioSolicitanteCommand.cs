using MediatR;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands
{
    public class RegistrarNovoUsuarioSolicitanteCommand : IRequest<Result<RegistrarNovoUsuarioSolicitanteResponse>>
    {
        public RegistrarNovoUsuarioSolicitanteCommand(string numeroDocumento, string emailSolicitante, string nomeSolicitante, string telefoneSolicitante)
        {
            NumeroDocumento = numeroDocumento;
            EmailSolicitante = emailSolicitante;
            NomeSolicitante = nomeSolicitante;
            TelefoneSolicitante = telefoneSolicitante;
        }

        public string NumeroDocumento { get; }
        public string EmailSolicitante { get; }
        public string NomeSolicitante { get; }
        public string TelefoneSolicitante { get; }
    }
}