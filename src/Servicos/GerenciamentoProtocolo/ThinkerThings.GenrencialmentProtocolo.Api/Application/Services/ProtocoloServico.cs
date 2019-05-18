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

        public async Task<Result<Protocolo>> ConsultarProtocoloPorNumero(string numeroProtocolo)
        {
            if (string.IsNullOrEmpty(numeroProtocolo))
                return Result<Protocolo>.Fail($"{nameof(numeroProtocolo)} não preenchido para consulta");

            try
            {
                var protocolo = await _protocoloRepositorio.ConsultarProtocoloPorNumero(numeroProtocolo);
                return Result<Protocolo>.Ok(protocolo);
            }
            catch (Exception ex)
            {
                return Result<Protocolo>.Fail("");
            }
        }

        public async Task<Result<string>> GerarNumeroProtocolo()
        {
            try
            {
                var sulfixoNumeroProtocolo = await _protocoloRepositorio.ObterProximoNumeroProtocolo().ConfigureAwait(false);
                if (sulfixoNumeroProtocolo <= 0)
                    return Result<string>.Fail($"Método ObterProximoNumeroProtocolo retornou menor ou igual a zero. Valor {sulfixoNumeroProtocolo}");

                var prefixoNumeroProtocolo = DateTimeOffset.Now.Year.ToString().PadRight(11, '0');

                var proximoNumeroProtocolo = prefixoNumeroProtocolo.Remove(prefixoNumeroProtocolo.Length - sulfixoNumeroProtocolo.ToString().Length);

                proximoNumeroProtocolo = proximoNumeroProtocolo.Insert(proximoNumeroProtocolo.Length, sulfixoNumeroProtocolo.ToString());
                return Result<string>.Ok(proximoNumeroProtocolo);
            }
            catch (Exception ex)
            {
                const string erroMessage = "Falha ao gerar o número de protocolo";

                _logger.LogError(erroMessage, ex);
                return Result<string>.Fail(erroMessage);
            }
        }

        public async Task<Result> RegistrarNovoProtocolo(Protocolo newProtocolo)
        {
            if (newProtocolo == null)
                return Result.Fail($"{nameof(newProtocolo)} não deve ser nulo.");

            if (newProtocolo.SolicitanteProtocolo == null)
                return Result.Fail($"{nameof(newProtocolo.SolicitanteProtocolo)} não deve ser nulo.");

            if (!newProtocolo.Detalhes.Any(x => x.ProtocoloDetalheItem.Key.Equals(ProtocoloDetalheItem.Solicitado.Key, StringComparison.InvariantCultureIgnoreCase)))
                return Result.Fail("");

            try
            {
                await _protocoloRepositorio.Registrar(newProtocolo).ConfigureAwait(false);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                const string erroMessage = "Falha ao registrar protocolo";

                _logger.LogError(erroMessage, ex);
                return Result<string>.Fail(erroMessage);
            }
        }
    }
}