using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Commands;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Handlers;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;

namespace ThinkerThings.Servicos.Usuarios.Conta.UnitTest.Application.Handers
{
    [TestFixture]
    public class RegistrarPreCadastroUsuarioHandlerTest
    {
        private IMediator mediator;
        private IUsuarioServico usuarioServico;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            usuarioServico = Substitute.For<IUsuarioServico>();
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Command_For_Nulo()
        {
            //Arrange
            var sut = new RegistrarPreCadastroUsuarioHandler(mediator, usuarioServico);

            //Act
            var response = await sut.Handle(null, default(CancellationToken));

            //Assert
            response.IsFailure.Should().BeTrue();
            response.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Command_For_Invalido()
        {
            //Arrange
            var command = FakeData.RegistrarPreCadastroUsuarioCommandInvalido;
            var sut = new RegistrarPreCadastroUsuarioHandler(mediator, usuarioServico);

            //Act
            var response = await sut.Handle(command, default(CancellationToken));

            //Assert
            response.IsFailure.Should().BeTrue();
            response.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Usuario_Ja_Cadastrado()
        {
            //Arrange

            usuarioServico.VerificarUsuarioJaCadastrado(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_ => Result<SituacaoCadastroUsuario>.Ok(SituacaoCadastroUsuario.UsuarioJaCadastrado));

            var command = FakeData.RegistrarPreCadastroUsuarioCommandValido;
            var sut = new RegistrarPreCadastroUsuarioHandler(mediator, usuarioServico);

            //Act
            var response = await sut.Handle(command, default(CancellationToken));

            //Assert
            response.IsFailure.Should().BeTrue();
            response.IsSuccess.Should().BeFalse();
            response.Value.Should().NotBe(SituacaoCadastroUsuario.UsuarioNaoCadastrado);
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Verificar_Usuario_Ja_Cadastrado_Lancar_Excessao()
        {
            //Arrange

            usuarioServico.VerificarUsuarioJaCadastrado(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_ => Task.FromException(new Exception()));

            var command = FakeData.RegistrarPreCadastroUsuarioCommandValido;
            var sut = new RegistrarPreCadastroUsuarioHandler(mediator, usuarioServico);

            //Act
            var response = await sut.Handle(command, default(CancellationToken));

            //Assert
            response.IsFailure.Should().BeTrue();
            response.IsSuccess.Should().BeFalse();
        }

        [TearDown]
        public void TearDown()
        {
            usuarioServico = null;
        }
    }

    internal static partial class FakeData
    {
        public static RegistrarPreCadastroUsuarioCommand RegistrarPreCadastroUsuarioCommandValido
        {
            get
            {
                return new Faker<RegistrarPreCadastroUsuarioCommand>()
                    .CustomInstantiator(f => new RegistrarPreCadastroUsuarioCommand(f.Person.FullName, f.Person.Email, f.Person.Cpf(), f.Person.Phone));
            }
        }

        public static RegistrarPreCadastroUsuarioCommand RegistrarPreCadastroUsuarioCommandInvalido
        {
            get
            {
                return new Faker<RegistrarPreCadastroUsuarioCommand>()
                    .CustomInstantiator(f => new RegistrarPreCadastroUsuarioCommand(string.Empty, string.Empty, string.Empty, string.Empty));
            }
        }
    }
}