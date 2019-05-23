using FluentValidation;
using ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.AggregateModels.UsuarioModel;

namespace ThinkerThings.Servicos.Gerenciador.Usuarios.Api.Application.Validators
{
    public class RegistrarNovoUsuarioCommandValidator : AbstractValidator<RegistrarNovoUsuarioCommand>
    {
        public RegistrarNovoUsuarioCommandValidator()
        {
            RuleFor(command => command.NomeUsuario).NotEmpty().Length(3, 30);
            RuleFor(command => command.CpfUsuario).NotEmpty();
            RuleFor(command => command.TelefoneUsuario).NotEmpty();
            RuleFor(command => command.EmailUsuario).NotEmpty().EmailAddress();
        }
    }
}
