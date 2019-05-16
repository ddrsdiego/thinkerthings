using FluentValidation;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Validators
{
    public class IniciarAtendimentoProtocoloCommandValidator : AbstractValidator<IniciarAtendimentoProtocoloCommand>
    {
        public IniciarAtendimentoProtocoloCommandValidator()
        {
            RuleFor(command => command.NumeroProtocolo).NotEmpty();
        }
    }
}