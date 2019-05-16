using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Services;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;

namespace ThinkerThings.GenrencialmentoProtocolo.Test.Application.Services
{
    [TestFixture]
    public class ProtocoloServicoTest
    {
        private ILoggerFactory loggerFactory;
        private IProtocoloRepositorio protocoloRepositorio;

        [SetUp]
        public void SetUp()
        {
            loggerFactory = Substitute.For<ILoggerFactory>();
            protocoloRepositorio = Substitute.For<IProtocoloRepositorio>();
        }

        [Test]
        public async Task Teste()
        {
            //Arrange
            var servico = new ProtocoloServico(protocoloRepositorio, loggerFactory);

            //Act
            var numeroProtocolo = await servico.GerarNumeroProtocolo().ConfigureAwait(false);

            //Assert
            Assert.IsNotEmpty(numeroProtocolo.Value);
        }

        [TearDown]
        public void TearDown()
        {
            protocoloRepositorio = null;
        }
    }
}