using MediatR;
using ThinkerThings.Servicos.Usuarios.Domain.AggregateModel.SeedWorks;
using ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Responses;

namespace ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Commands
{
    public class RegistrarNovoUsuarioCommand : IRequest<Result<RegistrarNovoUsuarioResponse>>
    {
        public RegistrarNovoUsuarioCommand(string numeroDocumento, string emailSolicitante, string nomeSolicitante, string telefoneSolicitante)
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