using Microsoft.Extensions.DependencyInjection;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Services;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Extensions.IoC
{
    public static class ServicoContainer
    {
        public static IServiceCollection AddServicos(this IServiceCollection services)
        {
            services.AddTransient<IProtocoloServico, ProtocoloServico>();
            services.AddTransient<IUsuarioSolicitanteServico, UsuarioSolicitanteServico>();

            return services;
        }
    }
}