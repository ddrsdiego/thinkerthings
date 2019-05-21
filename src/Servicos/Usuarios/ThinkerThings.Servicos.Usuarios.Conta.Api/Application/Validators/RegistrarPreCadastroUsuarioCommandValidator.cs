using FluentValidation;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Validators
{
    public class RegistrarPreCadastroUsuarioCommandValidator : AbstractValidator<RegistrarPreCadastroUsuarioCommand>
    {
        public RegistrarPreCadastroUsuarioCommandValidator()
        {
            RuleFor(command => command.NomeUsuario).NotEmpty().Length(3, 30);
            RuleFor(command => command.CpfUsuario).NotEmpty();
            RuleFor(command => command.TelefoneUsuario).NotEmpty();
            RuleFor(command => command.EmailUsuario).NotEmpty().EmailAddress();
        }
    }
}