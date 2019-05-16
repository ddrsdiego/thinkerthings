using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Services
{
    public class UsuarioSolicitanteServico : IUsuarioSolicitanteServico
    {
        private readonly ILogger<UsuarioSolicitanteServico> _logger;
        private readonly IUsuarioSolicitanteRepositorio _usuarioSolicitanteRepositorio;

        public UsuarioSolicitanteServico(IUsuarioSolicitanteRepositorio usuarioSolicitanteRepositorio, ILoggerFactory loggerFactory)
        {
            _usuarioSolicitanteRepositorio = usuarioSolicitanteRepositorio;
            _logger = loggerFactory.CreateLogger<UsuarioSolicitanteServico>();
        }

        public async Task<Result<UsuarioSolicitante>> ConsultarUsuarioSolicitante(string numeroDocumento, string email)
        {
            if (numeroDocumento == null)
                return Result<UsuarioSolicitante>.Fail($"{nameof(numeroDocumento)} não deve ser branco ou nulo.");

            if (email == null)
                return Result<UsuarioSolicitante>.Fail($"{nameof(email)} não deve ser branco ou nulo.");

            try
            {
                var usuarioSolicitante = await _usuarioSolicitanteRepositorio.ConsultarUsuarioSolicitante(numeroDocumento, email);
                if (usuarioSolicitante == null)
                    return Result<UsuarioSolicitante>.Fail("");

                return Result<UsuarioSolicitante>.Ok(usuarioSolicitante);
            }
            catch (Exception ex)
            {
                return Result<UsuarioSolicitante>.Fail("");
            }
        }

        public async Task<Result> RegistrarUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante)
        {
            if (usuarioSolicitante == null)
                return Result.Fail("");

            try
            {
                await _usuarioSolicitanteRepositorio.RegistrarUsuarioSolicitante(usuarioSolicitante);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("");
            }
        }
    }
}
