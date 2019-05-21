using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Infra.Options;
using ThinkerThings.Servicos.Usuarios.Conta.Infra.Statements;

namespace ThinkerThings.Servicos.Usuarios.Conta.Infra.Repositorios
{
    public class UsuarioRepositorio : RepositorioSqlServer, IUsuarioRepositorio
    {
        public UsuarioRepositorio(ILoggerFactory loggerFactory, IOptions<ConnectionStringOptions> connectionStringOptions)
            : base(loggerFactory.CreateLogger<UsuarioRepositorio>(), connectionStringOptions)
        {
        }

        public Task<Usuario> ConsultarUsuarioPorCpf(string cpfUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ConsultarUsuarioPorEmail(string emailUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ConsultarUsuarioPorId(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task RegistrarNovoUsuario(Usuario usuario)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.ExecuteAsync(UsuarioStatement.RegistrarNovoUsuario,
                        new
                        {

                        }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("", ex);
                throw;
            }
        }
    }
}