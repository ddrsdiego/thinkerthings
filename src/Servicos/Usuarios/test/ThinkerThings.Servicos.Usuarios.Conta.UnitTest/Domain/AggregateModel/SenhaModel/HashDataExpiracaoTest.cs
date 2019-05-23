using FluentAssertions;
using NUnit.Framework;
using System;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.SeedWorks;
using ThinkerThings.Servicos.Usuarios.Conta.UnitTest.Application.Servicos;

namespace ThinkerThings.Servicos.Usuarios.Conta.UnitTest.Domain.AggregateModel.SenhaModel
{
    [TestFixture]
    public class HashDataExpiracaoTest
    {
        [Test]
        public void Teste()
        {
            var usuairio = FakeData.UsuarioValido;
            var dataRequisicao = DateTimeOffset.Now;

            var sut = HashDataExpiracao.Criar(dataRequisicao, 8);

            sut.Value.DataExpiracao.Should().Be(dataRequisicao.AddHours(8));

            sut.Value.EhValido.Should().Be(Result.Ok());
        }
    }
}