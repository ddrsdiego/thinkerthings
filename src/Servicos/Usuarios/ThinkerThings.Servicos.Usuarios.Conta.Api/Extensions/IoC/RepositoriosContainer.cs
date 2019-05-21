using Microsoft.Extensions.DependencyInjection;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Infra.Repositorios;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Extensions.IoC
{
    internal static class RepositoriosContainer
    {
        public static IServiceCollection AddRepositorios(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IRequisicaoSenhaUsuarioRepositorio, RequisicaoSenhaUsuarioRepositorio>();

            return services;
        }
    }
}