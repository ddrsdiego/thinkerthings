using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
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
        public async Task Deve_Retornar_Falha_Quando_Repositorio_Lancar_Excessao()
        {
            //Arrange
            protocoloRepositorio.ObterProximoNumeroProtocolo().Returns(_ => Task.FromException<int>(new Exception()));
            var sut = new ProtocoloServico(protocoloRepositorio, loggerFactory);

            //Act
            var numeroProtocoloResult = await sut.GerarNumeroProtocolo().ConfigureAwait(false);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(numeroProtocoloResult);
                Assert.IsTrue(numeroProtocoloResult.IsFailure);
                Assert.IsFalse(numeroProtocoloResult.IsSuccess);
                Assert.IsNull(numeroProtocoloResult.Value);
            });
        }

        [Test]
        public async Task Deve_Retornar_Falha_Quando_Repositorio_Retornar_Proximo_Valor_Menor_Igual_Zero()
        {
            //Arrange
            protocoloRepositorio.ObterProximoNumeroProtocolo().Returns(_ => new Random().Next(-100, 0));
            var sut = new ProtocoloServico(protocoloRepositorio, loggerFactory);

            //Act
            var numeroProtocoloResult = await sut.GerarNumeroProtocolo().ConfigureAwait(false);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(numeroProtocoloResult);
                Assert.IsTrue(numeroProtocoloResult.IsFailure);
                Assert.IsFalse(numeroProtocoloResult.IsSuccess);
                Assert.IsNull(numeroProtocoloResult.Value);
            });
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000)]
        public async Task Deve_Retornar_Proximo_Numero_Protocolo_Com_Tamanho_11_Caracters(int proximoNumero)
        {
            //Arrange
            protocoloRepositorio.ObterProximoNumeroProtocolo().Returns(_ => proximoNumero);
            var sut = new ProtocoloServico(protocoloRepositorio, loggerFactory);

            //Act
            var numeroProtocolo = await sut.GerarNumeroProtocolo().ConfigureAwait(false);

            //Assert
            Assert.IsNotNull(numeroProtocolo);
            Assert.IsNotEmpty(numeroProtocolo.Value);
            Assert.AreEqual(numeroProtocolo.Value.Length, 11);
        }

        [TearDown]
        public void TearDown()
        {
            protocoloRepositorio = null;
        }
    }
}