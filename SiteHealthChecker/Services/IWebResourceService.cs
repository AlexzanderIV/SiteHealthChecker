using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteHealthChecker.Models;

namespace SiteHealthChecker.Services
{
    public interface IWebResourceService
    {
        Task<WebResource[]> GetWebResourcesAsync();

        Task<WebResource> GetWebResourceAsync(Guid id);

        Task<bool> CreateWebResourceAsync(WebResource newWebResource);

        Task<bool> UpdateWebResourceAsync(WebResource newWebResource);

        Task<bool> DeleteWebResourceAsync(Guid id);
    }
}
