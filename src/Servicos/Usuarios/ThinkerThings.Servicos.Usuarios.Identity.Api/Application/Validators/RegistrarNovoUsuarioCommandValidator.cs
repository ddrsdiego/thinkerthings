using FluentValidation;
using ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Commands;

namespace ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Validators
{
    public class RegistrarNovoUsuarioCommandValidator : AbstractValidator<RegistrarNovoUsuarioCommand>
    {
        public RegistrarNovoUsuarioCommandValidator()
        {
            RuleFor(command => command.NomeSolicitante).NotEmpty().Length(3, 30);
            RuleFor(command => command.EmailSolicitante).NotEmpty().EmailAddress();
            RuleFor(command => command.NumeroDocumento).NotEmpty();
            RuleFor(command => command.TelefoneSolicitante).NotEmpty();
        }
    }
}