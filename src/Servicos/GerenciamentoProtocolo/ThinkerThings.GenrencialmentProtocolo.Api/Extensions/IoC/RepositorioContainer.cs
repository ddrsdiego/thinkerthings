using Microsoft.Extensions.DependencyInjection;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.ProtocoloModel;
using ThinkerThings.GerenciamentoProtocolo.Domain.AggregateModels.UsuarioModel;
using ThinkerThings.GerenciamentoProtocolo.Infra.Repositorios;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Extensions.IoC
{
    public static class RepositorioContainer
    {
        public static IServiceCollection AddRepositorios(this IServiceCollection services)
        {
            services.AddScoped<IProtocoloRepositorio, ProtocoloRepositorio>();
            services.AddScoped<IUsuarioSolicitanteRepositorio, UsuarioSolicitanteRepositorio>();

            return services;
        }
    }
}
