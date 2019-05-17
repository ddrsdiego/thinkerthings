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
            services.AddTransient<IProtocoloRepositorio, ProtocoloRepositorio>();
            services.AddTransient<IUsuarioSolicitanteRepositorio, UsuarioSolicitanteRepositorio>();
            return services;
        }
    }
}
