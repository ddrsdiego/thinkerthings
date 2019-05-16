using System.Threading.Tasks;

namespace ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel
{
    public interface IProtocoloRepositorio
    {
        Task Registrar(Protocolo newProtocolo);
    }
}