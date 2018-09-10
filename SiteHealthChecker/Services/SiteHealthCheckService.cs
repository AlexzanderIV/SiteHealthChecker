using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SiteHealthChecker.Services
{
    public class SiteHealthCheckService : HostedService
    {
        private readonly SiteStatusProvider _siteStatusProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public SiteHealthCheckService(
            SiteStatusProvider siteStatusProvider, 
            IServiceScopeFactory scopeFactory, 
            IConfiguration configuration)
        {
            _siteStatusProvider = siteStatusProvider;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var checkInterval = (int)_configuration.GetValue(typeof(int), "CheckHealthInterval", 30);

            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var webResourceService = scope.ServiceProvider.GetRequiredService<IWebResourceService>();
                
                    var webResources = webResourceService.GetWebResourcesAsync().Result;
                    foreach (var webResource in webResources)
                    {
                        (var statusCode, var isAvailable) = await _siteStatusProvider.GetStatus(webResource.Uri, cancellationToken);

                        if (webResource.StatusCode != statusCode)
                        {
                            webResource.StatusCode = statusCode;
                            webResource.IsAvailable = isAvailable;
                            await webResourceService.UpdateWebResourceAsync(webResource);
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(checkInterval), cancellationToken);
                }
            }
        }
    }
}
