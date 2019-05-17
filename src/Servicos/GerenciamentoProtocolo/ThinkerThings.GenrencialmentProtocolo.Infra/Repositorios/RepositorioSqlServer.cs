using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace ThinkerThings.GerenciamentoProtocolo.Infra.Repositorios
{
    public abstract class RepositorioSqlServer
    {
        private readonly IOptions<ConnectionStringOptions> _connectionString;
        protected RepositorioSqlServer(ILogger logger, IOptions<ConnectionStringOptions> connectionString)
        {
            Logger = logger;
            _connectionString = connectionString;
        }

        protected ILogger Logger { get; }
        protected IDbConnection GetConnection() => new SqlConnection(ConnectionString);
        protected string ConnectionString => _connectionString.Value.ConnectionString;
    }
}