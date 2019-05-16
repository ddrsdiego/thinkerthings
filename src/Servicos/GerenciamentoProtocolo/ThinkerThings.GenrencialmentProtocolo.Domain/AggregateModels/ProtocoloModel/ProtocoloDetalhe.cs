using System;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel
{
    public class ProtocoloDetalhe
    {
        public ProtocoloDetalhe(ProtocoloDetalheItem protocoloDetalheItem)
        {
            ProtocoloDetalheItem = protocoloDetalheItem;
        }

        public int ProtocoloDetalheId { get; set; }
        public DateTimeOffset DataHoraRegistro => DateTimeOffset.Now;
        public ProtocoloDetalheItem ProtocoloDetalheItem { get; }
    }

    public class ProtocoloDetalheItem
    {
        public ProtocoloDetalheItem(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; }
        public string Message { get; }

        public static ProtocoloDetalheItem Solicitado = new ProtocoloDetalheItem("SOLICITADO", "Protocolo Solicitado");
        public static ProtocoloDetalheItem EmAtendimento = new ProtocoloDetalheItem("EMATENDIMENTO", "Protocolo em Atendimento");
        public static ProtocoloDetalheItem Finalizado = new ProtocoloDetalheItem("FINALIZADO", "Protocolo Finalizado");
        public static ProtocoloDetalheItem Cancelado = new ProtocoloDetalheItem("CANCELADO", "Protocolo Cancelado");
    }
}