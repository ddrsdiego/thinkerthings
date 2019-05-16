using System;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;

namespace ThinkerThings.GerenciamentoProtocolo.Infra.Repositorios
{
    public class UsuarioSolicitanteRepositorio : IUsuarioSolicitanteRepositorio
    {
        public Task<UsuarioSolicitante> ConsultarUsuarioSolicitante(string numeroDocumento, string email)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante)
        {
            throw new NotImplementedException();
        }
    }
}
