using System;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel
{
    public class RequisicaoSenhaUsuario
    {
        protected RequisicaoSenhaUsuario()
        {
        }

        private RequisicaoSenhaUsuario(Usuario usuario, DateTimeOffset dataRequisicao)
        {
            Usuario = usuario;
            DataRequisicao = dataRequisicao;
        }

        public Usuario Usuario { get; }
        public Guid HashRequisicaoSenha { get; } = Guid.NewGuid();
        public DateTimeOffset DataRequisicao { get; }
        public DateTimeOffset DataExpiracao { get; set; }

        public static Result<RequisicaoSenhaUsuario> Criar(Usuario usuario, DateTimeOffset dataRequisicao)
        {
            if (dataRequisicao == default(DateTimeOffset))
                Result<RequisicaoSenhaUsuario>.Fail(nameof(dataRequisicao));

            if (usuario == null)
                Result<RequisicaoSenhaUsuario>.Fail(nameof(usuario));

            if (usuario.UsuarioId <= 0)
                Result<RequisicaoSenhaUsuario>.Fail(nameof(usuario));

            return Result<RequisicaoSenhaUsuario>.Ok(new RequisicaoSenhaUsuario(usuario, dataRequisicao));
        }
    }
}