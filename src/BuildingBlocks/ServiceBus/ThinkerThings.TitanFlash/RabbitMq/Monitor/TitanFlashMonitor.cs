using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Net;
using ThinkerThings.TitanFlash.Bus.Contracts;

namespace ThinkerThings.TitanFlash.RabbitMq.Monitor
{
    public class TitanFlashMonitor : ITitanFlashMonitor
    {
        private readonly Uri _uriApi;
        private readonly ILogger _logger;
        private readonly IConnectionFactory _connectionFactory;

        public static TitanFlashMonitor Create(IConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
        {
            return new TitanFlashMonitor(connectionFactory, loggerFactory);
        }

        private TitanFlashMonitor(IConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(TitanFlashMonitor)) ?? throw new ArgumentException(nameof(loggerFactory));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

            if (connectionFactory != null)
                _uriApi = new Uri($"http://{connectionFactory.Uri.Host}:1{connectionFactory.Uri.Port}/api");
        }

        public bool HealthCheck()
        {
            try
            {
                using (var client = new WebClientWithoutKeepAlive())
                {
                    client.Credentials = new NetworkCredential(_connectionFactory.UserName, _connectionFactory.Password);

                    var data = client.DownloadString($"{_uriApi.AbsoluteUri}/healthchecks/node");
                    var info = JsonConvert.DeserializeObject<HealthCheckData>(data);

                    var isAlive = info.Status.Equals("ok", StringComparison.InvariantCultureIgnoreCase);

                    if (!isAlive)
                        _logger.LogError($"Não foi possível conectar no Rabbit pelo motivo: {info.Reason}");

                    return isAlive;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao tentar chamar o HealthCheck", ex);
                return false;
            }
        }
    }

    internal class WebClientWithoutKeepAlive : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var req = (HttpWebRequest)base.GetWebRequest(address);
            req.KeepAlive = false;
            return req;
        }
    }
}