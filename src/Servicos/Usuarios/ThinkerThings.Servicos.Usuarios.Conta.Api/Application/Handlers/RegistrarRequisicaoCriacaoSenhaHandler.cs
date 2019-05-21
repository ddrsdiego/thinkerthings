using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Responses;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Validators;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Handlers
{
    public class RegistrarRequisicaoCriacaoSenhaHandler : IRequestHandler<RegistrarRequisicaoCriacaoSenhaCommand, Result<RegistrarRequisicaoCriacaoSenhaResponse>>
    {
        private readonly IUsuarioServico _usuarioServico;
        private readonly IRequisicaoSenhaUsuarioServico _requisicaoSenhaUsuarioServico;

        public RegistrarRequisicaoCriacaoSenhaHandler(IUsuarioServico usuarioServico, IRequisicaoSenhaUsuarioServico requisicaoSenhaUsuarioServico)
        {
            _usuarioServico = usuarioServico;
            _requisicaoSenhaUsuarioServico = requisicaoSenhaUsuarioServico;
        }

        public async Task<Result<RegistrarRequisicaoCriacaoSenhaResponse>> Handle(RegistrarRequisicaoCriacaoSenhaCommand request, CancellationToken cancellationToken)
        {
            var commanValidator = new RegistrarRequisicaoCriacaoSenhaCommandValidator();
            var validatorResult = commanValidator.Validate(request);
            if (!validatorResult.IsValid)
                return Result<RegistrarRequisicaoCriacaoSenhaResponse>.Fail(validatorResult.Errors.Select(x => x.ErrorMessage));

            var consultaUsuarioResult = await _usuarioServico.ConsultarUsuarioPorId(request.UsuarioId);
            if (consultaUsuarioResult.IsFailure)
                return Result<RegistrarRequisicaoCriacaoSenhaResponse>.Fail(consultaUsuarioResult.Messages);

            var requisicaoSenhaUsuario = RequisicaoSenhaUsuario.Criar(consultaUsuarioResult.Value, DateTimeOffset.Now);
            var registrarRequisicaoSenhaResult = await _requisicaoSenhaUsuarioServico.RegistrarRequisicaoSenha(requisicaoSenhaUsuario.Value);
            if (registrarRequisicaoSenhaResult.IsFailure)
                return Result<RegistrarRequisicaoCriacaoSenhaResponse>.Fail(registrarRequisicaoSenhaResult.Messages);

            return Result<RegistrarRequisicaoCriacaoSenhaResponse>.Ok(new RegistrarRequisicaoCriacaoSenhaResponse());
        }
    }
}