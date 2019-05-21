using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using ThinkerThings.Servicos.Usuarios.Conta.Infra.Options;

namespace ThinkerThings.Servicos.Usuarios.Conta.Infra.Repositorios
{
    public abstract class RepositorioSqlServer
    {
        private readonly ConnectionStringOptions _connectionStringOptions;

        protected RepositorioSqlServer(ILogger logger, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            _connectionStringOptions = connectionStringOptions.Value;
            Logger = logger;
        }

        public ILogger Logger { get; }
        protected IDbConnection GetConnection() => new SqlConnection(_connectionStringOptions.ConnectionString);
    }
}