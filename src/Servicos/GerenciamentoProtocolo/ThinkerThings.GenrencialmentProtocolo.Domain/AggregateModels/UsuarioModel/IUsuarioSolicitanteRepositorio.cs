using System.Threading.Tasks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel
{
    public interface IUsuarioSolicitanteRepositorio
    {
        Task RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante);

        Task<UsuarioSolicitante> ConsultarUsuarioSolicitantePorCPF(string cpfSolicitante);

        Task<UsuarioSolicitante> ConsultarUsuarioSolicitantePorEmail(string emailSolicitante);
    }
}