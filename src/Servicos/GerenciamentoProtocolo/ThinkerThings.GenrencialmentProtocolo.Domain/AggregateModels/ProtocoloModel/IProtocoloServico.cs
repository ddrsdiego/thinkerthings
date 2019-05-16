using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel
{
    public interface IProtocoloServico
    {
        Task<Result<string>> GerarNumeroProtocolo();

        Task<Result> SolicitarProtocolo(Protocolo newProtocolo);

        Task<Result> Alterar(Protocolo newProtocolo);

        Task<Result<Protocolo>> ConsultarProtocoloPorNumero(string numeroProtocolo);
    }
}