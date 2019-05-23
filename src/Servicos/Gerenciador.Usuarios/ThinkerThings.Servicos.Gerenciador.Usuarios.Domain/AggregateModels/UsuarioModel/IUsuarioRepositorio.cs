using System.Threading.Tasks;

namespace ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.AggregateModels.UsuarioModel
{
    public interface IUsuarioRepositorio
    {
        Task RegistrarNovoUsuario(Usuario usuario);

        Task<Usuario> ConsultarUsuarioPorId(int usuarioId);

        Task<Usuario> ConsultarUsuarioPorEmail(string emailUsuario);

        Task<Usuario> ConsultarUsuarioPorCpf(string cpfUsuario);
    }
}