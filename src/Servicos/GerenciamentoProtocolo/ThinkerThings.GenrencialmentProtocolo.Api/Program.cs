using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ThinkerThings.GerenciamentoProtocolo.Api
{
    public class Program
    {
        private static IConfiguration Configuration { get; }
        private static readonly string Namespace = typeof(Program).Namespace;
        private static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static void Main(string[] args)
        {
            //Log.Logger = CreateSerilogLogger(Configuration);

            var host = CreateWebHostBuilder(args);

            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];

            return new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .MinimumLevel.Verbose()
                    .Enrich.WithProperty("ApplicationContext", AppName)
                    .Enrich.FromLogContext()
                    .CreateLogger();
        }
    }
}