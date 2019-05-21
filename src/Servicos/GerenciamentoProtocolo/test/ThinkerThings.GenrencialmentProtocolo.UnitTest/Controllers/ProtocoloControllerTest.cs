using MediatR;
using NUnit.Framework;

namespace ThinkerThings.GerenciamentoProtocolo.UnitTest.Controllers
{
    [TestFixture]
    public class ProtocoloControllerTest
    {
        private IMediator mediator;

        [SetUp]
        public void SetUp()
        {
            mediator = NSubstitute.Substitute.For<IMediator>();
        }
    }
}