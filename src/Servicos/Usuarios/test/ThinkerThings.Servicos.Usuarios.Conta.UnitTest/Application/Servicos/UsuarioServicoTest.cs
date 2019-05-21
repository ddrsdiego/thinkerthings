using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Servicos;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;

namespace ThinkerThings.Servicos.Usuarios.Conta.UnitTest.Application.Servicos
{
    [TestFixture]
    public class UsuarioServicoTest
    {
        private IUsuarioRepositorio usuarioRepositorio;

        [SetUp]
        public void SetUp()
        {
            usuarioRepositorio = Substitute.For<IUsuarioRepositorio>();
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Usuario_Para_Pre_Cadastro_For_Nulo()
        {
            //Arrange
            var sut = new UsuarioServico(usuarioRepositorio);

            //Act
            var response = await sut.RegistrarNovoUsuario(null);

            //Assert
            response.IsFailure.Should().BeTrue();
            response.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Repositorio_Lancar_Excessao()
        {
            //Arrange
            usuarioRepositorio
                .When(x => x.RegistrarNovoUsuario(Arg.Any<Usuario>()))
                .Do(_ => throw new Exception());

            var usuarioPreCadstro = FakeData.UsuarioValido;
            var sut = new UsuarioServico(usuarioRepositorio);

            //Act
            var response = await sut.RegistrarNovoUsuario(usuarioPreCadstro);

            //Assert
            response.IsFailure.Should().BeTrue();
            response.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task Deve_Retornar_Sucesso_Quando_Usuario_Nao_Estiver_Cadastrado()
        {
            var cpfUsuario = new Person().Cpf();
            var emailUsuario = new Person().Email;

            //Arrange
            usuarioRepositorio.ConsultarUsuarioPorEmail(Arg.Any<string>())
                .Returns(ReturnUsuarioNull());

            usuarioRepositorio.ConsultarUsuarioPorCpf(Arg.Any<string>())
                .Returns(ReturnUsuarioNull());

            var usuarioPreCadstro = FakeData.UsuarioValido;
            var sut = new UsuarioServico(usuarioRepositorio);

            //Act
            var response = await sut.VerificarUsuarioJaCadastrado(cpfUsuario, emailUsuario);

            //Assert
            response.IsFailure.Should().BeFalse();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().Be(SituacaoCadastroUsuario.UsuarioNaoCadastrado);
        }

        [Test]
        public async Task Deve_Retornar_Sucesso_Quando_Email_Ja_Estiver_Cadastrado()
        {
            var cpfUsuario = new Person().Cpf();
            var emailUsuario = new Person().Email;

            //Arrange
            usuarioRepositorio.ConsultarUsuarioPorEmail(Arg.Any<string>())
                .Returns(_ => new Usuario { Email = emailUsuario, CPF = cpfUsuario });

            usuarioRepositorio.ConsultarUsuarioPorCpf(Arg.Any<string>())
                .Returns(ReturnUsuarioNull());

            var usuarioPreCadstro = FakeData.UsuarioValido;
            var sut = new UsuarioServico(usuarioRepositorio);

            //Act
            var response = await sut.VerificarUsuarioJaCadastrado(cpfUsuario, emailUsuario);

            //Assert
            response.IsFailure.Should().BeFalse();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().Be(SituacaoCadastroUsuario.EmailJaCadastrado);
        }

        [Test]
        public async Task Deve_Retornar_Sucesso_Quando_CPF_Ja_Estiver_Cadastrado()
        {
            var cpfUsuario = new Person().Cpf();
            var emailUsuario = new Person().Email;

            //Arrange
            usuarioRepositorio.ConsultarUsuarioPorEmail(Arg.Any<string>())
                .Returns(ReturnUsuarioNull());

            usuarioRepositorio.ConsultarUsuarioPorCpf(Arg.Any<string>())
                .Returns(_ => new Usuario { Email = emailUsuario, CPF = cpfUsuario });

            var usuarioPreCadstro = FakeData.UsuarioValido;
            var sut = new UsuarioServico(usuarioRepositorio);

            //Act
            var response = await sut.VerificarUsuarioJaCadastrado(cpfUsuario, emailUsuario);

            //Assert
            response.IsFailure.Should().BeFalse();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().Be(SituacaoCadastroUsuario.CpfJaCadastrado);
        }

        [Test]
        public async Task Deve_Retornar_Sucesso_Quando_Usuario_Ja_Estiver_Cadastrado()
        {
            var cpfUsuario = new Person().Cpf();
            var emailUsuario = new Person().Email;

            //Arrange
            usuarioRepositorio.ConsultarUsuarioPorEmail(Arg.Any<string>())
                .Returns(_ => new Usuario { Email = emailUsuario, CPF = cpfUsuario });

            usuarioRepositorio.ConsultarUsuarioPorCpf(Arg.Any<string>())
                .Returns(_ => new Usuario { Email = emailUsuario, CPF = cpfUsuario });

            var usuarioPreCadstro = FakeData.UsuarioValido;
            var sut = new UsuarioServico(usuarioRepositorio);

            //Act
            var response = await sut.VerificarUsuarioJaCadastrado(cpfUsuario, emailUsuario);

            //Assert
            response.IsFailure.Should().BeFalse();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().Be(SituacaoCadastroUsuario.UsuarioJaCadastrado);
        }

        [Test]
        public async Task Deve_Retornar_Sucesso_Quando_Repositorio_Registra_Pre_Cadastro()
        {
            //Arrange
            var usuarioPreCadstro = FakeData.UsuarioValido;
            var sut = new UsuarioServico(usuarioRepositorio);

            //Act
            var response = await sut.RegistrarNovoUsuario(usuarioPreCadstro);

            //Assert
            response.IsFailure.Should().BeFalse();
            response.IsSuccess.Should().BeTrue();

            usuarioRepositorio.Received().RegistrarNovoUsuario(Arg.Any<Usuario>()).GetAwaiter().GetResult();
        }

        private static Usuario ReturnUsuarioNull() => null;

        [TearDown]
        public void TearDown()
        {
            usuarioRepositorio = null;
        }
    }

    internal static partial class FakeData
    {
        private const string LOCALE = "pt_BR";

        public static Usuario UsuarioValido
        {
            get
            {
                return new Faker<Usuario>(LOCALE)
                        .RuleFor(x => x.Nome, f => f.Name.FullName())
                        .RuleFor(x => x.Email, f => f.Person.Email)
                        .RuleFor(x => x.CPF, f => f.Person.Cpf())
                        .RuleFor(x => x.Telefone, f => f.Person.Phone)
                        .RuleFor(x => x.UsuarioId, f => f.Random.Number(1, 10));
            }
        }
    }
}