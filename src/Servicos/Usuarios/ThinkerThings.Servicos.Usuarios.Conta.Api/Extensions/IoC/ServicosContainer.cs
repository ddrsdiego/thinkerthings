using Microsoft.Extensions.DependencyInjection;
using ThinkerThings.Servicos.Usuarios.Conta.Api.Application.Servicos;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;

namespace ThinkerThings.Servicos.Usuarios.Conta.Api.Extensions.IoC
{
    internal static class ServicosContainer
    {
        public static IServiceCollection AddServicos(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioServico, UsuarioServico>();
            services.AddTransient<IRequisicaoSenhaUsuarioServico, RequisicaoSenhaUsuarioServico>();
            return services;
        }
    }
}