using System;
using System.Collections.Generic;
using System.Linq;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel
{
    public class Protocolo
    {
        private List<ProtocoloDetalhe> _protocoloDetalhes;

        public Protocolo()
        {
            StatusProtocolo = StatusProtocolo.Solicitado;
            _protocoloDetalhes = new List<ProtocoloDetalhe>();
        }

        public string NumeroProtocolo { get; set; }
        public StatusProtocolo StatusProtocolo { get; set; }
        public ProtocoloUsuarioSolicitante SolicitanteProtocolo { get; set; }
        public IReadOnlyCollection<ProtocoloDetalhe> Detalhes => _protocoloDetalhes;

        public DateTimeOffset DataSolicitacao { get; } = DateTimeOffset.Now;
        public DateTimeOffset DataInicializacao { get; private set; }

        public Result InicializarProtocolo()
        {
            if (StatusProtocolo != StatusProtocolo.Solicitado)
                return Result.Fail($"{nameof(ProtocoloModel.StatusProtocolo)} não esta no status de solicitado.");

            StatusProtocolo = StatusProtocolo.Inicializado;
            DataInicializacao = DateTimeOffset.Now;

            return Result.Ok();
        }

        public Result AdicionarDetalhe(ProtocoloDetalhe protocoloDetalhe)
        {
            var detalhe = _protocoloDetalhes.SingleOrDefault(x => x.ProtocoloDetalheId == protocoloDetalhe.ProtocoloDetalheId);
            if (detalhe != null)
                Result.Fail($"{protocoloDetalhe.ProtocoloDetalheItem.Message} já adicionado.");

            _protocoloDetalhes.Add(protocoloDetalhe);
            return Result.Ok();
        }
    }
}