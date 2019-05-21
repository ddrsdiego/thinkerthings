using Bogus;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Handlres;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;

namespace ThinkerThings.GerenciamentoProtocolo.UnitTest.Application.Handlers
{
    [TestFixture]
    public class RegistrarNovoUsuarioSolicitanteHandlerTest
    {
        private IUsuarioSolicitanteServico usuarioSolicitanteServico;

        [SetUp]
        public void SetUp()
        {
            usuarioSolicitanteServico = Substitute.For<IUsuarioSolicitanteServico>();
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Request_For_Invalido()
        {
            //Arrange
            var sut = new RegistrarNovoUsuarioSolicitanteHandler(usuarioSolicitanteServico);

            //Act
            var response = await sut.Handle(FakeData.RegistrarNovoUsuarioSolicitanteCommandInvalido, default(CancellationToken));

            //Assert
        }

        [TearDown]
        public void TearDown()
        {
            usuarioSolicitanteServico = null;
        }
    }

    internal static partial class FakeData
    {
        public static RegistrarNovoUsuarioSolicitanteCommand RegistrarNovoUsuarioSolicitanteCommandInvalido =>
            new Faker<RegistrarNovoUsuarioSolicitanteCommand>(LOCALE)
                    .CustomInstantiator(_ => new RegistrarNovoUsuarioSolicitanteCommand(string.Empty, string.Empty, string.Empty, string.Empty));
    }
}