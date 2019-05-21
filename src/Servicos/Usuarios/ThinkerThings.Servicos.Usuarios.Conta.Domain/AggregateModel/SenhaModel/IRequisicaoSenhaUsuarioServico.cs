using System;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel
{
    public interface IRequisicaoSenhaUsuarioServico
    {
        Result ValidarHashRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario);
        Task<Result> RegistrarRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario);
        Task<Result<RequisicaoSenhaUsuario>> ConsultarRequisicaoPorHash(Guid hashRequisicaoSenha);
    }
}