using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel
{
    public interface IUsuarioSolicitanteServico
    {
        Task<Result> RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante);

        Task<Result<UsuarioSolicitante>> ConsultarUsuarioSolicitantePorCPF(string cpfSolicitante);

        Task<Result<UsuarioSolicitante>> ConsultarUsuarioSolicitantePorEmail(string emailUsuarioSolicitante);
    }
}