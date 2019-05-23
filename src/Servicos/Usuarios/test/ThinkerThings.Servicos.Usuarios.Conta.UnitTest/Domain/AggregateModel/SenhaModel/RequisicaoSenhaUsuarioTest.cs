using FluentAssertions;
using NUnit.Framework;
using System;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.UnitTest.Application.Servicos;

namespace ThinkerThings.Servicos.Usuarios.Conta.UnitTest.Domain.AggregateModel.SenhaModel
{
    [TestFixture]
    public class RequisicaoSenhaUsuarioTest
    {
        [Test]
        public void Test()
        {
            var usuairio = FakeData.UsuarioValido;
            var sut = RequisicaoSenhaUsuario.Criar(usuairio, DateTimeOffset.Now);

            sut.IsSuccess.Should().BeTrue();
            sut.IsFailure.Should().BeFalse();
            sut.Value.Usuario.UsuarioId.Should().Be(usuairio.UsuarioId);
        }
    }
}