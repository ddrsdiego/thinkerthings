using System;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses
{
    public class SolicitarAtendimentoResponse
    {
        public SolicitarAtendimentoResponse(string novoNumeroProtocolo, DateTimeOffset dataSolicitacao)
        {
            DataSolicitacao = dataSolicitacao;
            NovoNumeroProtocolo = novoNumeroProtocolo;
        }

        public DateTimeOffset DataSolicitacao { get; }
        public string NovoNumeroProtocolo { get; }
    }
}
