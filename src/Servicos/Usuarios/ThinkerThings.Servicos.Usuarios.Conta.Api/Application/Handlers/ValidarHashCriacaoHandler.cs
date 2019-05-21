using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Responses;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Handlers
{
    public class ValidarHashCriacaoHandler : IRequestHandler<ValidarHashCriacaoCommand, Result<ValidarHashCriacaoResponse>>
    {
        private readonly IRequisicaoSenhaUsuarioServico _requisicaoSenhaUsuarioServico;

        public ValidarHashCriacaoHandler(IRequisicaoSenhaUsuarioServico requisicaoSenhaUsuarioServico)
        {
            _requisicaoSenhaUsuarioServico = requisicaoSenhaUsuarioServico;
        }

        public async Task<Result<ValidarHashCriacaoResponse>> Handle(ValidarHashCriacaoCommand request, CancellationToken cancellationToken)
        {
            var resultadoConsultaRequisicaoPorHash = await ConsultarRequisicaoPorHash(request);
            if (resultadoConsultaRequisicaoPorHash.IsFailure)
                return Result<ValidarHashCriacaoResponse>.Fail(resultadoConsultaRequisicaoPorHash.Messages);

            var resultadoValidacaoHash = ValidarHashRequisicaoSenha(resultadoConsultaRequisicaoPorHash.Value);
            if (resultadoValidacaoHash.IsFailure)
                return Result<ValidarHashCriacaoResponse>.Fail(resultadoValidacaoHash.Messages);

            return Result<ValidarHashCriacaoResponse>.Ok(new ValidarHashCriacaoResponse());
        }

        private async Task<Result<RequisicaoSenhaUsuario>> ConsultarRequisicaoPorHash(ValidarHashCriacaoCommand request)
        {
            try
            {
                var consultaHashResult = await _requisicaoSenhaUsuarioServico.ConsultarRequisicaoPorHash(request.HashRequisicaoSenha);
                if (consultaHashResult.IsFailure)
                    return Result<RequisicaoSenhaUsuario>.Fail(consultaHashResult.Messages);

                return Result<RequisicaoSenhaUsuario>.Ok(consultaHashResult.Value);
            }
            catch (Exception ex)
            {
                return Result<RequisicaoSenhaUsuario>.Fail(ex.Message);
            }
        }

        private Result ValidarHashRequisicaoSenha(RequisicaoSenhaUsuario requisicaoSenhaUsuario)
        {
            try
            {
                var validacaoHashResult = _requisicaoSenhaUsuarioServico.ValidarHashRequisicaoSenha(requisicaoSenhaUsuario);
                if (validacaoHashResult.IsFailure)
                    return Result.Fail(validacaoHashResult.Messages);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.ToString());
            }
        }
    }
}