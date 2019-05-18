using Microsoft.Extensions.DependencyInjection;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Extensions.IoC
{
    public static class GerenciamentoProtocoloContainer
    {
        public static IServiceCollection AddGerenciamentoProtocoloServices(this IServiceCollection services)
        {
            services.AddSwagger();
            services.AddHandlers();
            services.AddServicos();
            services.AddRepositorios();
            return services;
        }
    }
}