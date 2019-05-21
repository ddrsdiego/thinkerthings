using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Extensions.IoC
{
    public static class ServicosUsuariosContaContainer
    {
        public static IServiceCollection AddUsuariosContaServicos(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}