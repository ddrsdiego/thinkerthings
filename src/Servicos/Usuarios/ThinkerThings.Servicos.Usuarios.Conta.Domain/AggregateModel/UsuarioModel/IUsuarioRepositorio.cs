using System.Threading.Tasks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel
{
    public interface IUsuarioRepositorio
    {
        Task RegistrarNovoUsuario(Usuario usuario);

        Task<Usuario> ConsultarUsuarioPorId(int usuarioId);

        Task<Usuario> ConsultarUsuarioPorEmail(string emailUsuario);

        Task<Usuario> ConsultarUsuarioPorCpf(string cpfUsuario);
    }
}