using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Services
{
    public class ProtocoloServico : IProtocoloServico
    {
        private readonly ILogger<ProtocoloServico> _logger;
        private readonly IProtocoloRepositorio _protocoloRepositorio;

        public ProtocoloServico(IProtocoloRepositorio protocoloRepositorio, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProtocoloServico>();
            _protocoloRepositorio = protocoloRepositorio ?? throw new ArgumentNullException(nameof(protocoloRepositorio));
        }

        public Task<Result> Alterar(Protocolo newProtocolo)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Protocolo>> ConsultarProtocoloPorNumero(string numeroProtocolo)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> GerarNumeroProtocolo()
        {
            try
            {
                await Task.CompletedTask;
                return Result<string>.Ok(string.Empty);
            }
            catch (Exception ex)
            {
                const string erroMessage = "Falha ao gerar o número de protocolo";

                _logger.LogError(erroMessage, ex);
                return Result<string>.Fail(erroMessage);
            }
        }

        public async Task<Result> SolicitarProtocolo(Protocolo newProtocolo)
        {
            if (newProtocolo == null)
                return Result.Fail("");

            if (newProtocolo.SolicitanteProtocolo == null)
                return Result.Fail("");

            if (!newProtocolo.Detalhes.Any(x => x.ProtocoloDetalheItem.Key.Equals(ProtocoloDetalheItem.Solicitado.Key, StringComparison.InvariantCultureIgnoreCase)))
                return Result.Fail("");

            try
            {
                await _protocoloRepositorio.Registrar(newProtocolo).ConfigureAwait(false);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("");
            }
        }
    }
}