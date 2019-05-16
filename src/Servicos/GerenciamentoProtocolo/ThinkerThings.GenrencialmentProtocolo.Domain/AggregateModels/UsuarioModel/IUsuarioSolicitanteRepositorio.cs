using System.Threading.Tasks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel
{
    public interface IUsuarioSolicitanteRepositorio
    {
        Task RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante);

        Task<UsuarioSolicitante> ConsultarUsuarioSolicitante(string numeroDocumento, string email);
    }
}