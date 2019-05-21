using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel
{
    public interface IUsuarioServico
    {
        Task<Result> RegistrarNovoUsuario(Usuario usuario);

        Task<Result<Usuario>> ConsultarUsuarioPorId(int usuarioId);

        Task<Result<SituacaoCadastroUsuario>> VerificarUsuarioJaCadastrado(string cpfUsuario, string emailUsuario);
    }
}