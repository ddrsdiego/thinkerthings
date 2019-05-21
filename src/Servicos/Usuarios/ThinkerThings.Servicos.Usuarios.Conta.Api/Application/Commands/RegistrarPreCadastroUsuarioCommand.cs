using MediatR;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Responses;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands
{
    public class RegistrarPreCadastroUsuarioCommand : IRequest<Result<RegistrarPreCadastroUsuarioResponse>>
    {
        public RegistrarPreCadastroUsuarioCommand(string nomeUsuario, string emailUsuario, string cpfUsuario, string telefoneUsuario)
        {
            NomeUsuario = nomeUsuario;
            EmailUsuario = emailUsuario;
            CpfUsuario = cpfUsuario;
            TelefoneUsuario = telefoneUsuario;
        }

        public string NomeUsuario { get; }
        public string EmailUsuario { get; }
        public string CpfUsuario { get; }
        public string TelefoneUsuario { get; }
    }
}