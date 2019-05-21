using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Domain.AggregateModel.SeedWorks;
using ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Commands;
using ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Responses;
using ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Validators;

namespace ThinkerThings.Servicos.Usuarios.Identity.Api.Application.Handlers
{
    public class RegistrarNovoUsuarioHandler : IRequestHandler<RegistrarNovoUsuarioCommand, Result<RegistrarNovoUsuarioResponse>>
    {
        public RegistrarNovoUsuarioHandler()
        {
        }

        public async Task<Result<RegistrarNovoUsuarioResponse>> Handle(RegistrarNovoUsuarioCommand request, CancellationToken cancellationToken)
        {
            var validarRequestResult = ValidarRequest(request);
            if (validarRequestResult.IsFailure)
                return Result<RegistrarNovoUsuarioResponse>.Fail(validarRequestResult.Messages);

            await Task.CompletedTask;
            return Result<RegistrarNovoUsuarioResponse>.Ok(new RegistrarNovoUsuarioResponse());
        }

        private static Result ValidarRequest(RegistrarNovoUsuarioCommand request)
        {
            if (request == null)
                return Result.Fail($"{nameof(RegistrarNovoUsuarioCommand)} não deve ser nulo.");

            var validator = new RegistrarNovoUsuarioCommandValidator();

            var validarRequestResult = validator.Validate(request);
            if (!validarRequestResult.IsValid)
                return Result.Fail(validarRequestResult.Errors.Select(x => x.ErrorMessage));

            return Result.Ok();
        }
    }
}