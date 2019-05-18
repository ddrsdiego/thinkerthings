using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Handlres;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Responses;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.SeedWorks;

namespace ThinkerThings.GerenciamentoProtocolo.UnitTest.Application.Handlers
{
    [TestFixture]
    public class SolicitarAtendimentoHandlerTest
    {
        private IMediator mediator;
        private IProtocoloServico protocoloServico;
        private IUsuarioSolicitanteServico usuarioSolicitanteServico;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            protocoloServico = Substitute.For<IProtocoloServico>();
            usuarioSolicitanteServico = Substitute.For<IUsuarioSolicitanteServico>();
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Request_Invalido()
        {
            //Arrange
            var command = FakeData.SolicitarAtendimentoCommandInvalido;

            //Act
            var handler = new SolicitarAtendimentoHandler(mediator, protocoloServico, usuarioSolicitanteServico);
            var response = await handler.Handle(command, default(CancellationToken)).ConfigureAwait(false);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsInstanceOf<Result<SolicitarAtendimentoResponse>>(response);
                Assert.IsNull(response.Value);
                Assert.IsTrue(response.IsFailure);
                Assert.Greater(response.Messages.Count, 1);
            });
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Request_Nao_Contem_Nome()
        {
            //Arrange
            var handler = new SolicitarAtendimentoHandler(mediator, protocoloServico, usuarioSolicitanteServico);

            //Act
            var response = await handler.Handle(FakeData.SolicitarAtendimentoCommandSemNome, default(CancellationToken)).ConfigureAwait(false);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsInstanceOf<Result<SolicitarAtendimentoResponse>>(response);
                Assert.IsNull(response.Value);
                Assert.IsTrue(response.IsFailure);
                Assert.AreEqual(response.Messages.Count, 2);
            });
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Consultar_Usuario_Requisicao_Cadastro_Retornar_Null()
        {
            //Arrange
            usuarioSolicitanteServico.ConsultarUsuarioSolicitante(Arg.Any<string>(), Arg.Any<string>()).Returns(_ => Result<UsuarioSolicitante>.Fail(""));
            protocoloServico.GerarNumeroProtocolo().Returns(_ => Result<string>.Ok(Guid.NewGuid().ToString()));

            //Act
            var handler = new SolicitarAtendimentoHandler(mediator, protocoloServico, usuarioSolicitanteServico);
            var response = await handler.Handle(FakeData.SolicitarAtendimentoCommandValido, default(CancellationToken)).ConfigureAwait(false);

            //Assert
            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsInstanceOf<Result<SolicitarAtendimentoResponse>>(response);
                Assert.IsNull(response.Value);
                Assert.IsTrue(response.IsFailure);
                Assert.AreEqual(1, response.Messages.Count);
            });
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Consultar_Usuario_Solicitante_Requisicao_Cadastro_Retornar_Falha()
        {
            //Arrange
            protocoloServico.GerarNumeroProtocolo().Returns(_ => Result<string>.Ok(Guid.NewGuid().ToString()));
            mediator.Send(Arg.Any<RegistrarNovoUsuarioSolicitanteCommand>()).Returns(_ => Result<RegistrarNovoUsuarioSolicitanteResponse>.Fail(""));
            usuarioSolicitanteServico.ConsultarUsuarioSolicitante(Arg.Any<string>(), Arg.Any<string>()).Returns(_ => Result<UsuarioSolicitante>.Fail(""));

            //Act
            var sut = new SolicitarAtendimentoHandler(mediator, protocoloServico, usuarioSolicitanteServico);
            var response = await sut.Handle(FakeData.SolicitarAtendimentoCommandValido, default(CancellationToken)).ConfigureAwait(false);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsInstanceOf<Result<SolicitarAtendimentoResponse>>(response);
                Assert.IsNull(response.Value);
                Assert.IsTrue(response.IsFailure);
                Assert.AreEqual(1, response.Messages.Count);
            });
        }

        [Test]
        public async Task Deve_Falhar_Quando_RegistrarNovoProtocolo_Retonar_Falha()
        {
            //Arrange
            var novoNumeroProtocolo = Guid.NewGuid().ToString();
            var usuarioSolicitante = FakeData.SolicitarAtendimentoCommandValido;

            protocoloServico.GerarNumeroProtocolo()
                .Returns(_ => Result<string>.Ok(novoNumeroProtocolo));

            protocoloServico.RegistrarNovoProtocolo(Arg.Any<Protocolo>())
                .Returns(_ => Result.Fail(""));

            usuarioSolicitanteServico.ConsultarUsuarioSolicitante(Arg.Any<string>(), Arg.Any<string>()).Returns(_ => Result<UsuarioSolicitante>.Fail(""));

            mediator.Send(Arg.Any<RegistrarNovoUsuarioSolicitanteCommand>())
                .Returns(_ => Result<RegistrarNovoUsuarioSolicitanteResponse>.Ok(new RegistrarNovoUsuarioSolicitanteResponse
                {
                    NumeroDocumento = usuarioSolicitante.NumeroDocumento,
                    EmailSolicitante = usuarioSolicitante.EmailSolicitante
                }));

            //Act
            var sut = new SolicitarAtendimentoHandler(mediator, protocoloServico, usuarioSolicitanteServico);
            var response = await sut.Handle(FakeData.SolicitarAtendimentoCommandValido, default(CancellationToken)).ConfigureAwait(false);

            //Assert
            protocoloServico.Received().RegistrarNovoProtocolo(Arg.Any<Protocolo>()).GetAwaiter();
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsInstanceOf<Result<SolicitarAtendimentoResponse>>(response);
                Assert.IsNull(response.Value);
                Assert.IsTrue(response.IsFailure);
                Assert.AreEqual(1, response.Messages.Count);
            });
        }

        [Test]
        public async Task Deve_Retornar_Usuario_Solicitante_Quando_Requisitar_Novo_Cadastro_Usuario_Solicitante()
        {
            //Arrange
            var novoNumeroProtocolo = Guid.NewGuid().ToString();
            var usuarioSolicitante = FakeData.SolicitarAtendimentoCommandValido;
            protocoloServico.GerarNumeroProtocolo().Returns(_ => Result<string>.Ok(novoNumeroProtocolo));
            usuarioSolicitanteServico.ConsultarUsuarioSolicitante(Arg.Any<string>(), Arg.Any<string>()).Returns(_ => Result<UsuarioSolicitante>.Fail(""));

            mediator.Send(Arg.Any<RegistrarNovoUsuarioSolicitanteCommand>())
                .Returns(_ => Result<RegistrarNovoUsuarioSolicitanteResponse>.Ok(new RegistrarNovoUsuarioSolicitanteResponse
                {
                    NumeroDocumento = usuarioSolicitante.NumeroDocumento,
                    EmailSolicitante = usuarioSolicitante.EmailSolicitante
                }));

            //Act
            var sut = new SolicitarAtendimentoHandler(mediator, protocoloServico, usuarioSolicitanteServico);
            var response = await sut.Handle(FakeData.SolicitarAtendimentoCommandValido, default(CancellationToken)).ConfigureAwait(false);

            protocoloServico.Received().RegistrarNovoProtocolo(Arg.Any<Protocolo>()).GetAwaiter();
            response.Value.NovoNumeroProtocolo.Should().Be(novoNumeroProtocolo);
        }

        [TearDown]
        public void TearDown()
        {
            mediator = null;
            protocoloServico = null;
            usuarioSolicitanteServico = null;
        }
    }

    public static class FakeData
    {
        private const string LOCALE = "pt_BR";

        public static SolicitarAtendimentoCommand SolicitarAtendimentoCommandInvalido
        {
            get
            {
                return new Faker<SolicitarAtendimentoCommand>(LOCALE)
                    .CustomInstantiator(_ => new SolicitarAtendimentoCommand(string.Empty, string.Empty, string.Empty, string.Empty));
            }
        }

        public static SolicitarAtendimentoCommand SolicitarAtendimentoCommandSemNome
        {
            get
            {
                return new Faker<SolicitarAtendimentoCommand>(LOCALE)
                    .CustomInstantiator(f => new SolicitarAtendimentoCommand(string.Empty, f.Person.Email, f.Person.Phone, f.Person.Cpf()));
            }
        }

        public static SolicitarAtendimentoCommand SolicitarAtendimentoCommandValido
        {
            get
            {
                return new Faker<SolicitarAtendimentoCommand>(LOCALE)
                    .CustomInstantiator(f => new SolicitarAtendimentoCommand(f.Name.FullName(), f.Person.Email, f.Person.Phone, f.Person.Cpf()));
            }
        }
    }
}