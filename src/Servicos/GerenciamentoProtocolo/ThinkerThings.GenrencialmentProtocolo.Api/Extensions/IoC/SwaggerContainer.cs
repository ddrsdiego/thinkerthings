using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ThinkerThings.GerenciamentoProtocolo.Api.Extensions.IoC
{
    public static class SwaggerContainer
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Easynvest.Orders.FixedIncome",
                    Version = "v1",
                    Description = "Api para consulta a execução de ordens de renda fixa.",
                    Contact = new Contact { Url = "https://bitbucket.org/easynvest/easynvest.orders.FixedIncome" }
                });
                options.AddSecurityDefinition("bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Favor gerar um token jwt para efetuar a requisição",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
            });

            return services;
        }
    }
}
