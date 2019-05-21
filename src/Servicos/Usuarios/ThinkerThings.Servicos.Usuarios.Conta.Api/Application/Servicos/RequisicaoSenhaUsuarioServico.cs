using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Options;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Servicos
{
    public class RequisicaoSenhaUsuarioServico : IRequisicaoSenhaUsuarioServico
    {
        private readonly ConfiguracoesHash _configuracoesHash;
        private readonly IRequisicaoSenhaUsuarioRepositorio _requisicaoSenhaUsuarioRepositorio;

        public RequisicaoSenhaUsuarioServico(IRequisicaoSenhaUsuarioRepositorio requisicaoSenhaUsuarioRepositorio, IOptions<ConfiguracoesHash> configuracoesHashOptions)
        {
            _requisicaoSenhaUsuarioRepositorio = requisicaoSenhaUsuarioRepositorio;
            _configuracoesHash = configuracoesHashOptions.Value ?? throw new InvalidOperationException();
        }

        public async Task<Result<RequisicaoSenhaUsuario>> ConsultarRequisicaoPorHash(Guid hashRequisicaoSenha)
            => await ExecutarConsultaRequisicaoPorHash(hashRequisicaoSenha).ConfigureAwait(false);

        public async Task<Result> RegistrarRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario)
            => await ExecutarRegistorRequisicaoSenha(requisicaoSenhaUsuario);

        private async Task<Result<RequisicaoSenhaUsuario>> ExecutarConsultaRequisicaoPorHash(Guid hashRequisicaoSenha)
        {
            if (hashRequisicaoSenha == default(Guid))
                return Result<RequisicaoSenhaUsuario>.Fail("");

            try
            {
                var hash = await _requisicaoSenhaUsuarioRepositorio.ConsultarRequisicaoPorHash(hashRequisicaoSenha);
                if (hash == null)
                    return Result<RequisicaoSenhaUsuario>.Fail("");

                return Result<RequisicaoSenhaUsuario>.Ok(hash);
            }
            catch (Exception ex)
            {
                return Result<RequisicaoSenhaUsuario>.Fail("");
            }
        }

        private async Task<Result> ExecutarRegistorRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario)
        {
            if (requisicaoSenhaUsuario == null)
                return Result<RequisicaoSenhaUsuario>.Fail(nameof(requisicaoSenhaUsuario));

            try
            {
                await _requisicaoSenhaUsuarioRepositorio.RegistrarRequisicaoSenha(requisicaoSenhaUsuario).ConfigureAwait(false);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("");
            }
        }

        public Result ValidarHashRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario)
        {
            if (requisicaoSenhaUsuario == null)
                return Result.Fail("");

            if (_configuracoesHash.TempoExpiracaoEmHoras < 1)
                return Result.Fail("");

            try
            {
                if (requisicaoSenhaUsuario.DataExpiracao != default(DateTimeOffset))
                    return Result.Fail("");

                requisicaoSenhaUsuario.DataExpiracao
                    = requisicaoSenhaUsuario.DataRequisicao.AddHours(_configuracoesHash.TempoExpiracaoEmHoras);

                if (DateTimeOffset.Now >= requisicaoSenhaUsuario.DataExpiracao)
                    return Result.Fail("");

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("");
            }
        }
    }
}