namespace ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.AggregateModels.UsuarioModel
{
    public class RegistrarNovoUsuarioCommand : IRequest<Result<RegistrarNovoUsuarioResponse>>
    {
        public RegistrarNovoUsuarioCommand(string nomeUsuario, string emailUsuario, string cpfUsuario, string telefoneUsuario)
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