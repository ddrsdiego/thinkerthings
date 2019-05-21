using FluentValidation;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Validators
{
    public class RegistrarRequisicaoCriacaoSenhaCommandValidator : AbstractValidator<RegistrarRequisicaoCriacaoSenhaCommand>
    {
        public RegistrarRequisicaoCriacaoSenhaCommandValidator()
        {
            RuleFor(command => command.UsuarioId).GreaterThan(0).NotNull();
        }
    }
}