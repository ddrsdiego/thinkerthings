using MediatR;
using System;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Responses;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands
{
    public class ValidarHashCriacaoCommand : IRequest<Result<ValidarHashCriacaoResponse>>
    {
        public ValidarHashCriacaoCommand(Guid hashRequisicaoSenha)
        {
            HashRequisicaoSenha = hashRequisicaoSenha;
        }

        public Guid HashRequisicaoSenha { get; }
    }
}