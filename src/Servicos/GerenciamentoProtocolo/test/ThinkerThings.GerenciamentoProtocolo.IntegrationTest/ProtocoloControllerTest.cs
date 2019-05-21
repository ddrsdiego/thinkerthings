using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using ThinkerThings.GerenciamentoProtocolo.Api;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Commands;

namespace ThinkerThings.GerenciamentoProtocolo.IntegrationTest
{
    public class ProtocoloControllerTest
    {
        private TestServer _server;
        private HttpClient _client;

        [OneTimeSetUp]
        public void SetUp()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                            .UseStartup<Startup>()
                            .UseEnvironment("Development"));

            _client = _server.CreateClient();
        }

        public async Task Index_Get_ReturnsIndexHtmlPage()
        {
            // Act
            var command = FakeData.SolicitarAtendimentoCommandValido;
            var response = await _client.PostAsJsonAsync("/", FakeData.SolicitarAtendimentoCommandValido);

            // Assert
            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
            //Assert.Contains("<title>Home Page - BlogPlayground</title>", responseString);
        }
    }

    internal static partial class FakeData
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
