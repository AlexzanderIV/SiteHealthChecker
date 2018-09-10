using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SiteHealthChecker.Services
{
    public class SiteStatusProvider
    {
        private readonly HttpClient _httpClient;

        public SiteStatusProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(HttpStatusCode, bool)> GetStatus(string uri, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(uri, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return (response.StatusCode, true);
                }
                return (response.StatusCode, false);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                return (HttpStatusCode.ServiceUnavailable, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (HttpStatusCode.InternalServerError, false);
            }
        }
    }
}
