using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;

namespace ThinkerThings.GerenciamentoProtocolo.Infra.Repositorios
{
    public class UsuarioSolicitanteRepositorio : RepositorioSqlServer, IUsuarioSolicitanteRepositorio
    {
        public UsuarioSolicitanteRepositorio(ILoggerFactory loggerFactory, IOptions<ConnectionStringOptions> options)
            : base(loggerFactory.CreateLogger<UsuarioSolicitanteRepositorio>(), options)
        {
        }

        public async Task<UsuarioSolicitante> ConsultarUsuarioSolicitantePorCPF(string cpfSolicitante)
        {
            await Task.CompletedTask;

            return default(UsuarioSolicitante);
        }

        public async Task<UsuarioSolicitante> ConsultarUsuarioSolicitantePorEmail(string emailSolicitante)
        {
            await Task.CompletedTask;

            return default(UsuarioSolicitante);
        }

        public async Task RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante)
        {
            await Task.CompletedTask;
        }
    }
}