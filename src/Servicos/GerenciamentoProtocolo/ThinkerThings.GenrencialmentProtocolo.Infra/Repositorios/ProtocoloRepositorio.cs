using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;
using ThinkerThings.GerenciamentoProtocolo.Infra.Statements;

namespace ThinkerThings.GerenciamentoProtocolo.Infra.Repositorios
{
    public class ProtocoloRepositorio : RepositorioSqlServer, IProtocoloRepositorio
    {
        public ProtocoloRepositorio(ILoggerFactory loggerFactory, IOptions<ConnectionStringOptions> options)
            : base(loggerFactory.CreateLogger<ProtocoloRepositorio>(), options)
        {
        }

        public Task<Protocolo> ConsultarProtocoloPorNumero(string numeroProtocolo)
        {
            throw new NotImplementedException();
        }

        public async Task<int> ObterProximoNumeroProtocolo()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    return await conn.ExecuteScalarAsync<int>(ProtocoloRepositorioStatemets.ObterProximoNumeroProtocolo).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Registrar(Protocolo newProtocolo)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.ExecuteAsync(ProtocoloRepositorioStatemets.Registrar, new { }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Falha ao registrar novo protocolo", ex);
                throw;
            }
        }
    }
}