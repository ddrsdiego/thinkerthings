using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Responses;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Validators;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Handlers
{
    public class RegistrarPreCadastroUsuarioHandler : IRequestHandler<RegistrarPreCadastroUsuarioCommand, Result<RegistrarPreCadastroUsuarioResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioServico _usuarioServico;

        public RegistrarPreCadastroUsuarioHandler(IMediator mediator, IUsuarioServico usuarioServico)
        {
            _mediator = mediator;
            _usuarioServico = usuarioServico;
        }

        public async Task<Result<RegistrarPreCadastroUsuarioResponse>> Handle(RegistrarPreCadastroUsuarioCommand request, CancellationToken cancellationToken)
        {
            var validarRequestResult = ValidarRequest(request);
            if (validarRequestResult.IsFailure)
                return Result<RegistrarPreCadastroUsuarioResponse>.Fail(validarRequestResult.Messages);

            var verificarUsuarioJaCadastroResult = await VerificarUsuarioJaCadastrado(request);
            if (verificarUsuarioJaCadastroResult.IsFailure)
                return Result<RegistrarPreCadastroUsuarioResponse>.Fail(verificarUsuarioJaCadastroResult.Messages);

            if (verificarUsuarioJaCadastroResult.Value != SituacaoCadastroUsuario.UsuarioNaoCadastrado)
                return Result<RegistrarPreCadastroUsuarioResponse>.Fail(verificarUsuarioJaCadastroResult.Value.ToString());

            var novoUsuario = new Usuario
            {
                Nome = request.NomeUsuario,
                CPF = request.CpfUsuario,
                Email = request.EmailUsuario,
                Telefone = request.TelefoneUsuario,
            };

            var registrarNovoUsuarioResult = await RegistrarNovoUsuario(novoUsuario);
            if (registrarNovoUsuarioResult.IsFailure)
                return Result<RegistrarPreCadastroUsuarioResponse>.Fail(registrarNovoUsuarioResult.Messages);

            var registrarRequisicaoCriacaoSenhaResponse = await _mediator.Send(new RegistrarRequisicaoCriacaoSenhaCommand(novoUsuario.UsuarioId), cancellationToken);
            if (registrarRequisicaoCriacaoSenhaResponse.IsFailure)
                return Result<RegistrarPreCadastroUsuarioResponse>.Fail(registrarRequisicaoCriacaoSenhaResponse.Messages);

            return Result<RegistrarPreCadastroUsuarioResponse>.Ok(CriarResponse(novoUsuario));
        }

        private static RegistrarPreCadastroUsuarioResponse CriarResponse(Usuario novoUsuario)
        {
            return new RegistrarPreCadastroUsuarioResponse
            {
                UsuarioId = novoUsuario.UsuarioId,
                CpfUsuario = novoUsuario.CPF,
                EmailUsuario = novoUsuario.Email,
                NomeUsuario = novoUsuario.Nome
            };
        }

        private static Result ValidarRequest(RegistrarPreCadastroUsuarioCommand request)
        {
            if (request == null)
                return Result.Fail(nameof(request));

            var requestValidator = new RegistrarPreCadastroUsuarioCommandValidator();
            var resultValidator = requestValidator.Validate(request);
            if (!resultValidator.IsValid)
                return Result.Fail(resultValidator.Errors.Select(x => x.ErrorMessage));

            return Result.Ok();
        }

        private async Task<Result> RegistrarNovoUsuario(Usuario novoUsuario)
        {
            try
            {
                var registrarNovoUsuarioResult = await _usuarioServico.RegistrarNovoUsuario(novoUsuario);
                if (registrarNovoUsuarioResult.IsFailure)
                    return Result<RegistrarPreCadastroUsuarioResponse>.Fail(registrarNovoUsuarioResult.Messages);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result<RegistrarPreCadastroUsuarioResponse>.Fail("");
            }
        }

        private async Task<Result<SituacaoCadastroUsuario>> VerificarUsuarioJaCadastrado(RegistrarPreCadastroUsuarioCommand request)
        {
            try
            {
                var verificarUsuarioJaCadastroResult = await _usuarioServico.VerificarUsuarioJaCadastrado(request.CpfUsuario, request.EmailUsuario);
                if (verificarUsuarioJaCadastroResult.IsFailure)
                    return Result<SituacaoCadastroUsuario>.Fail(verificarUsuarioJaCadastroResult.Messages);

                return Result<SituacaoCadastroUsuario>.Ok(verificarUsuarioJaCadastroResult.Value);
            }
            catch (Exception ex)
            {
                return Result<SituacaoCadastroUsuario>.Fail("");
            }
        }
    }
}