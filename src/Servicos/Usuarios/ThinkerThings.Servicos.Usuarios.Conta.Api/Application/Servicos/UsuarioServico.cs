using System;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Servicos
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<Result<Usuario>> ConsultarUsuarioPorId(int usuarioId)
            => await ExecutarConsultaUsuarioPorId(usuarioId).ConfigureAwait(false);

        private async Task<Result<Usuario>> ExecutarConsultaUsuarioPorId(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioRepositorio.ConsultarUsuarioPorId(usuarioId);
                if (usuario == null)
                    return Result<Usuario>.Fail("");

                return Result<Usuario>.Ok(usuario);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Fail("");
            }
        }

        public async Task<Result> RegistrarNovoUsuario(Usuario usuario)
        {
            if (usuario == null)
                return Result.Fail(nameof(usuario));

            try
            {
                await _usuarioRepositorio.RegistrarNovoUsuario(usuario).ConfigureAwait(false);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("");
            }
        }

        public async Task<Result<SituacaoCadastroUsuario>> VerificarUsuarioJaCadastrado(string cpfUsuario, string emailUsuario)
        {
            if (cpfUsuario == null)
                return Result<SituacaoCadastroUsuario>.Fail(nameof(cpfUsuario));

            if (emailUsuario == null)
                return Result<SituacaoCadastroUsuario>.Fail(nameof(emailUsuario));

            try
            {
                var usuarioCpfTask = _usuarioRepositorio.ConsultarUsuarioPorCpf(cpfUsuario);
                var usuarioEmailTask = _usuarioRepositorio.ConsultarUsuarioPorEmail(emailUsuario);

                await Task.WhenAll(usuarioCpfTask, usuarioEmailTask);

                var usuarioCpf = await usuarioCpfTask.ConfigureAwait(false);
                var usuarioEmail = await usuarioEmailTask.ConfigureAwait(false);

                if (usuarioCpf != null && usuarioEmail != null)
                    return Result<SituacaoCadastroUsuario>.Ok(SituacaoCadastroUsuario.UsuarioJaCadastrado);

                if (usuarioEmail != null)
                    return Result<SituacaoCadastroUsuario>.Ok(SituacaoCadastroUsuario.EmailJaCadastrado);

                if (usuarioCpf != null)
                    return Result<SituacaoCadastroUsuario>.Ok(SituacaoCadastroUsuario.CpfJaCadastrado);

                return Result<SituacaoCadastroUsuario>.Ok(SituacaoCadastroUsuario.UsuarioNaoCadastrado);
            }
            catch (Exception ex)
            {
                return Result<SituacaoCadastroUsuario>.Fail("");
            }
        }
    }
}