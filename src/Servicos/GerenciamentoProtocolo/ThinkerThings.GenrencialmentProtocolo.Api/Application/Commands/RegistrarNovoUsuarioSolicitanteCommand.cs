using MediatR;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands
{
    public class RegistrarNovoUsuarioSolicitanteCommand : IRequest<Result<RegistrarNovoUsuarioSolicitanteResponse>>
    {
        public RegistrarNovoUsuarioSolicitanteCommand(string emailSolicitante, string nomeSolicitante, string telefoneSolicitante, string cpfSolicitante)
        {
            EmailSolicitante = emailSolicitante;
            NomeSolicitante = nomeSolicitante;
            TelefoneSolicitante = telefoneSolicitante;
            CPFSolicitante = cpfSolicitante;
        }

        public string EmailSolicitante { get; }
        public string NomeSolicitante { get; }
        public string TelefoneSolicitante { get; }
        public string CPFSolicitante { get; }
    }
}