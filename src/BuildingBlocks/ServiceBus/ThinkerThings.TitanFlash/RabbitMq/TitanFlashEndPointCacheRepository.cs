using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinkerThings.TitanFlash.Bus.Contracts;
using ThinkerThings.TitanFlash.Bus.Topology.Contracts;

namespace ThinkerThings.TitanFlash.RabbitMq
{
    public class TitanFlashEndPointCacheRepository : ITitanFlashEndPointRepository
    {
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public TitanFlashEndPointCacheRepository(IMemoryCache memoryCache, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _memoryCache = memoryCache;
            _logger = loggerFactory.CreateLogger<TitanFlashEndPointCacheRepository>();
        }

        public async Task<IEndPointConfigurator> Get(string queueName)
        {
            if (queueName == null)
                throw new System.ArgumentNullException(nameof(queueName));

            IEndPointConfigurator endPoint = null;

            return await Task.Run(() =>
            {
                var result = _memoryCache.TryGetValue(queueName, out endPoint);
                if (result)
                    return endPoint;
                return endPoint;
            });
        }

        public async Task Set(IEndPointConfigurator endPointsConfig)
        {
            var endPointCache = await Get(endPointsConfig.QueueConfiguration.Name).ConfigureAwait(false);
            if (endPointCache != null)
                _memoryCache.Remove(endPointCache.QueueConfiguration.Name);

            _logger.LogInformation($"Salvando EndPoint: {endPointsConfig.QueueConfiguration.Name} in Cache.");

            //_memoryCache.Set(endPointsConfig.QueueConfiguration.Name, endPointsConfig);

            await Task.CompletedTask;
        }

        public async Task Set(IEnumerable<IEndPointConfigurator> endPointsConfigs)
        {
            foreach (var endPoint in endPointsConfigs)
            {
                await Set(endPoint).ConfigureAwait(false);
            }
        }
    }
}