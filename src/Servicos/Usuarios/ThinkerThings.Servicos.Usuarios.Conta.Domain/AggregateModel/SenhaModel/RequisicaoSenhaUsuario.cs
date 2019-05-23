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
        public HashDataExpiracao HashDataExpiracao { get; }
        public Result HashDataExpiracaoEhValido => HashDataExpiracao.EhValido;

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

    public class HashDataExpiracao
    {
        protected HashDataExpiracao()
        {
        }

        private HashDataExpiracao(DateTimeOffset dataRequisicao, int tempoExpiracaoEmHoras)
        {
            TempoExpiracaoEmHoras = tempoExpiracaoEmHoras;
            DataRequisicao = dataRequisicao;
        }

        public static Result<HashDataExpiracao> Criar(DateTimeOffset dataRequisicao, int tempoExpiracaoEmHoras)
        {
            if (dataRequisicao == default(DateTimeOffset))
                return Result<HashDataExpiracao>.Fail(nameof(dataRequisicao));

            if (tempoExpiracaoEmHoras <= 0)
                return Result<HashDataExpiracao>.Fail(nameof(tempoExpiracaoEmHoras));

            return Result<HashDataExpiracao>.Ok(new HashDataExpiracao(dataRequisicao, tempoExpiracaoEmHoras));
        }

        public int TempoExpiracaoEmHoras { get; }
        public DateTimeOffset DataRequisicao { get; }
        public DateTimeOffset DataExpiracao => DataRequisicao.AddHours(TempoExpiracaoEmHoras);
        public Result EhValido
        {
            get
            {
                if (DataExpiracao == default(DateTimeOffset))
                    return Result.Fail("");

                if (DateTimeOffset.Now >= DataExpiracao)
                    return Result.Fail("");

                return Result.Ok();
            }
        }
    }
}