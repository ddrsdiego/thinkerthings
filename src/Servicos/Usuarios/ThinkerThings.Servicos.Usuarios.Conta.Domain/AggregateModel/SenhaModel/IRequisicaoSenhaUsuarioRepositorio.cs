using System;
using System.Threading.Tasks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel
{
    public interface IRequisicaoSenhaUsuarioRepositorio
    {
        Task RegistrarRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario);

        Task<RequisicaoSenhaUsuario> ConsultarRequisicaoPorHash(Guid hashRequisicaoSenha);
    }
}