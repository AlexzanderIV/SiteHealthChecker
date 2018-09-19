using SiteHealthChecker.Data;
using SiteHealthChecker.Models;
using SiteHealthChecker.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace SiteHealthChecker.UnitTests
{
    public class WebResourceServiceShould
    {
        private static readonly DbContextOptions<ApplicationDbContext> _options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_WebResources").Options;

        [Fact(DisplayName = "CreateWebResourceAsync should add new WebResource with StatusCode = 0")]
        public async void CreateWebResourceWithStatusCodeZero()
        {
            const string fakeWebResourceName = "Test web resource 1";
            var fakeWebResource = new WebResource
            {
                Name = fakeWebResourceName,
                Uri = "http://test.test",
            };
            bool result;

            // Set up a context (connection to the "DB") for writing.
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new WebResourceService(context);

                result = await service.CreateWebResourceAsync(fakeWebResource);
            }

            // Use a separate context to read data back from the "DB".
            using (var context = new ApplicationDbContext(_options))
            {
                var webResourcesCount = await context.WebResources.CountAsync();
                Assert.Equal(1, webResourcesCount);

                var webResource = await context.WebResources.FirstAsync();

                Assert.True(result);
                Assert.NotEqual(Guid.Empty, webResource.Id);
                Assert.Equal(fakeWebResourceName, webResource.Name);
                Assert.True((DateTimeOffset.Now - webResource.LastUpdated).TotalSeconds < 1);
                Assert.Equal(0, (int)webResource.StatusCode);
            }
        }
    }
}
