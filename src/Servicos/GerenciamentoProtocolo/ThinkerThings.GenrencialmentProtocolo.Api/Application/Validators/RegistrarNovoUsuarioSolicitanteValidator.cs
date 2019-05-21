using FluentValidation;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Validators
{
    public class RegistrarNovoUsuarioSolicitanteCommandValidator : AbstractValidator<RegistrarNovoUsuarioSolicitanteCommand>
    {
        public RegistrarNovoUsuarioSolicitanteCommandValidator()
        {
            RuleFor(command => command.NomeSolicitante).NotEmpty().Length(3, 30);
            RuleFor(command => command.EmailSolicitante).NotEmpty().EmailAddress();
            RuleFor(command => command.TelefoneSolicitante).NotEmpty();
        }
    }
}