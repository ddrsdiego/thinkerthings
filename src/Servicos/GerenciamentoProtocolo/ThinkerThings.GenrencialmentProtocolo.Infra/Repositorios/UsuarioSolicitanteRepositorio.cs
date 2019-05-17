using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
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

        public Task<UsuarioSolicitante> ConsultarUsuarioSolicitante(string numeroDocumento, string email)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante)
        {
            throw new NotImplementedException();
        }
    }
}