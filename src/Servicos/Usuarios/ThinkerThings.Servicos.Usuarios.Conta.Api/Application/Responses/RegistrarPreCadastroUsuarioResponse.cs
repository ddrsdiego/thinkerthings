namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Responses
{
    public class RegistrarPreCadastroUsuarioResponse
    {
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string CpfUsuario { get; set; }
    }
}