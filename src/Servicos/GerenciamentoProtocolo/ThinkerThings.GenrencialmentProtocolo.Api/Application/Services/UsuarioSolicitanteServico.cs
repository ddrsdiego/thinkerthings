using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Application.Services
{
    public class UsuarioSolicitanteServico : IUsuarioSolicitanteServico
    {
        private bool _disposed = false;
        private readonly ILogger<UsuarioSolicitanteServico> _logger;
        private readonly IUsuarioSolicitanteRepositorio _usuarioSolicitanteRepositorio;

        public UsuarioSolicitanteServico(IUsuarioSolicitanteRepositorio usuarioSolicitanteRepositorio, ILoggerFactory loggerFactory)
        {
            _usuarioSolicitanteRepositorio = usuarioSolicitanteRepositorio;
            _logger = loggerFactory.CreateLogger<UsuarioSolicitanteServico>();
        }

        public async Task<Result<UsuarioSolicitante>> ConsultarUsuarioSolicitantePorCPF(string cpfSolicitante)
        {
            if (cpfSolicitante == null)
                return Result<UsuarioSolicitante>.Fail($"{nameof(cpfSolicitante)} não deve ser branco ou nulo.");

            try
            {
                return ResultadoConsultaUsuarioSolicitante(await _usuarioSolicitanteRepositorio.ConsultarUsuarioSolicitantePorCPF(cpfSolicitante));
            }
            catch (Exception ex)
            {
                return Result<UsuarioSolicitante>.Fail("");
            }
        }

        public async Task<Result<UsuarioSolicitante>> ConsultarUsuarioSolicitantePorEmail(string emailUsuarioSolicitante)
            => await ExecutarConsultaUsuarioSolicitantePorEmail(emailUsuarioSolicitante).ConfigureAwait(false);

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

        private async Task<Result<UsuarioSolicitante>> ExecutarConsultaUsuarioSolicitantePorEmail(string emailUsuarioSolicitante)
        {
            if (emailUsuarioSolicitante == null)
                return Result<UsuarioSolicitante>.Fail($"{nameof(emailUsuarioSolicitante)} não deve ser branco ou nulo.");

            try
            {
                return ResultadoConsultaUsuarioSolicitante(await _usuarioSolicitanteRepositorio.ConsultarUsuarioSolicitantePorEmail(emailUsuarioSolicitante));
            }
            catch (Exception ex)
            {
                return Result<UsuarioSolicitante>.Fail("");
            }
        }

        private static Result<UsuarioSolicitante> ResultadoConsultaUsuarioSolicitante(UsuarioSolicitante usuarioSolicitante)
        {
            if (usuarioSolicitante == null)
                return Result<UsuarioSolicitante>.Fail("");

            return Result<UsuarioSolicitante>.Ok(usuarioSolicitante);
        }
    }
}