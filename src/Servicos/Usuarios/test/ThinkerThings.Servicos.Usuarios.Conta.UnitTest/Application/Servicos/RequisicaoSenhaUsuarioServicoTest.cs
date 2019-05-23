using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Servicos;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Options;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;

namespace ThinkerThings.Servicos.Usuarios.Conta.UnitTest.Application.Servicos
{
    [TestFixture]
    public class RequisicaoSenhaUsuarioServicoTest
    {
        private IOptions<ConfiguracoesHash> configuracoesHashOptions;
        private IRequisicaoSenhaUsuarioRepositorio requisicaoSenhaUsuarioRepositorio;

        [SetUp]
        public void SetUp()
        {
            configuracoesHashOptions = Substitute.For<IOptions<ConfiguracoesHash>>();
            requisicaoSenhaUsuarioRepositorio = Substitute.For<IRequisicaoSenhaUsuarioRepositorio>();
        }

        [Test]
        public void Teste()
        {
            //Arrange
            const int TEMPO_EXPIRACAO_EM_HORAS = 8;

            configuracoesHashOptions.Value
                .Returns(_ => new ConfiguracoesHash { TempoExpiracaoEmHoras = TEMPO_EXPIRACAO_EM_HORAS });

            var sut = new RequisicaoSenhaUsuarioServico(requisicaoSenhaUsuarioRepositorio, configuracoesHashOptions);
            var requisicaoSenha = RequisicaoSenhaUsuario.Criar(FakeData.UsuarioValido, DateTimeOffset.Now);

            //Act
            var result = sut.ValidarHashRequisicaoSenha(requisicaoSenha.Value);

            //Arrange
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Test]
        public void Teste11()
        {
            //Arrange
            const int TEMPO_EXPIRACAO_EM_HORAS = 8;

            configuracoesHashOptions.Value
                .Returns(_ => new ConfiguracoesHash { TempoExpiracaoEmHoras = TEMPO_EXPIRACAO_EM_HORAS });

            var sut = new RequisicaoSenhaUsuarioServico(requisicaoSenhaUsuarioRepositorio, configuracoesHashOptions);
            var requisicaoSenha = RequisicaoSenhaUsuario.Criar(FakeData.UsuarioValido, DateTimeOffset.Now.AddHours(-9));

            //Act
            var result = sut.ValidarHashRequisicaoSenha(requisicaoSenha.Value);

            //Arrange
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void DataExpiracao()
        {
            //Arrange
            const int TEMPO_EXPIRACAO_EM_HORAS = 8;

            configuracoesHashOptions.Value
                .Returns(_ => new ConfiguracoesHash { TempoExpiracaoEmHoras = TEMPO_EXPIRACAO_EM_HORAS });

            var sut = new RequisicaoSenhaUsuarioServico(requisicaoSenhaUsuarioRepositorio, configuracoesHashOptions);
            var requisicaoSenha = RequisicaoSenhaUsuario.Criar(FakeData.UsuarioValido, DateTimeOffset.Now.AddHours(-1));

            requisicaoSenha.Value.DataExpiracao = DateTimeOffset.Now;

            //Act
            var result = sut.ValidarHashRequisicaoSenha(requisicaoSenha.Value);

            //Arrange
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [TearDown]
        public void TearDown()
        {
            requisicaoSenhaUsuarioRepositorio = null;
        }
    }
}
