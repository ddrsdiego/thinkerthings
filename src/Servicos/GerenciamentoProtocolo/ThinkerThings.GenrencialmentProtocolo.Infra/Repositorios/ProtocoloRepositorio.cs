using System;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;

namespace ThinkerThings.GenrencialmentoProtocolo.Infra.Repositorios
{
    public class ProtocoloRepositorio : IProtocoloRepositorio
    {
        public ProtocoloRepositorio()
        {
        }

        public Task Registrar(Protocolo newProtocolo)
        {
            throw new NotImplementedException();
        }
    }
}