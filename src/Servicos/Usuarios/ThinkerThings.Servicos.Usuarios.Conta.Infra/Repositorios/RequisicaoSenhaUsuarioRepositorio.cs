using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Infra.Options;
using ThinkerThings.Servicos.Usuarios.Conta.Infra.Statements;

namespace ThinkerThings.Servicos.Usuarios.Conta.Infra.Repositorios
{
    public class RequisicaoSenhaUsuarioRepositorio : RepositorioSqlServer, IRequisicaoSenhaUsuarioRepositorio
    {
        public RequisicaoSenhaUsuarioRepositorio(ILoggerFactory loggerFactory, IOptions<ConnectionStringOptions> connectionStringOptions)
            : base(loggerFactory.CreateLogger<RequisicaoSenhaUsuarioRepositorio>(), connectionStringOptions)
        {
        }

        public async Task<RequisicaoSenhaUsuario> ConsultarRequisicaoPorHash(Guid hashRequisicaoSenha)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    return await conn.QuerySingleOrDefaultAsync(RequisicaoSenhaUsuarioStatement.ConsultarRequisicaoPorHash,
                        new
                        {
                            hashRequisicaoSenha
                        }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task RegistrarRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario)
        {
            await Task.CompletedTask;

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(RequisicaoSenhaUsuarioStatement.RegistrarRequisicaoSenha, new { });
            }
        }
    }
}