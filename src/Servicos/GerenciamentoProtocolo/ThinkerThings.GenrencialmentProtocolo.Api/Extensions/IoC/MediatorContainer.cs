using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ThinkerThings.GerenciamentoProtocolo.Api.Application.Handlres;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Extensions.IoC
{
    public static class MediatorContainer
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services
                .AddMediatR(typeof(IMediator).GetTypeInfo().Assembly)
                .AddMediatR(typeof(IRequestHandler<>).GetTypeInfo().Assembly)
                .AddMediatR(typeof(SolicitarAtendimentoHandler).Assembly);

            return services;
        }
    }
}