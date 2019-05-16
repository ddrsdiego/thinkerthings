using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel
{
    public interface IUsuarioSolicitanteServico
    {
        Task<Result> RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante);

        Task<Result<UsuarioSolicitante>> ConsultarUsuarioSolicitante(string numeroDocumento, string email);
    }
}