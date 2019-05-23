using System.Threading.Tasks;
using ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.AggregateModels.UsuarioModel
{
    public interface IUsuarioServico
    {
        Task<Result> RegistrarNovoUsuario(Usuario usuario);

        Task<Result<Usuario>> ConsultarUsuarioPorId(int usuarioId);

        Task<Result<SituacaoCadastroUsuario>> VerificarUsuarioJaCadastrado(string cpfUsuario, string emailUsuario);
    }
}
