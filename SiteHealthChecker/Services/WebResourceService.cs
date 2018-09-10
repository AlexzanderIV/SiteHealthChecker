using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteHealthChecker.Data;
using SiteHealthChecker.Models;

namespace SiteHealthChecker.Services
{
    public class WebResourceService : IWebResourceService
    {
        private readonly ApplicationDbContext _context;

        public WebResourceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WebResource[]> GetWebResourcesAsync()
        {
            var webResources = await _context.WebResources.ToArrayAsync();

            return webResources;
        }

        public async Task<WebResource> GetWebResourceAsync(Guid id)
        {
            var webResource = await _context.WebResources
                .FirstOrDefaultAsync(x => x.Id == id);

            return webResource;
        }

        public async Task<bool> CreateWebResourceAsync(WebResource newWebResource)
        {
            newWebResource.Id = Guid.NewGuid();
            newWebResource.StatusCode = 0;
            newWebResource.LastUpdated = DateTimeOffset.Now;

            _context.WebResources.Add(newWebResource);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> UpdateWebResourceAsync(WebResource webResource)
        {
            webResource.LastUpdated = DateTimeOffset.Now;

            _context.WebResources.Update(webResource);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> DeleteWebResourceAsync(Guid id)
        {
            var webResource = await _context.WebResources
                .SingleOrDefaultAsync(x => x.Id == id);

            if (webResource == null)
            {
                return false;
            }

            _context.WebResources.Remove(webResource);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}
