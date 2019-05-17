using System.Threading.Tasks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel
{
    public interface IProtocoloRepositorio
    {
        Task<Protocolo> ConsultarProtocoloPorNumero(string numeroProtocolo);

        Task Registrar(Protocolo newProtocolo);

        Task<int> ObterProximoNumeroProtocolo();
    }
}