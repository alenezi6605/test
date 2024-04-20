using TestsService.Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Net.Http.Headers;

namespace TestsService.External
{
    public interface IHttpClient
    {
        public Task<T> GetRequest<T>(string uri, IDictionary<string, string>? headers = null, bool isCashe = true);

        public uint CacheTime();
    }
    public class HttpClient : IHttpClient
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpClient> _logger;
        public HttpClient(IMemoryCache memoryCache, IConfiguration configuration, ILogger<HttpClient> logger)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<T> GetRequest<T>(string uri, IDictionary<string, string>? headers, bool isCashe = true)
        {

            T result;

            string? headerString = headers != null ? string.Join(",", headers.Select(kv => kv.Key + "=" + kv.Value).ToArray()) : null;

            if (_memoryCache.TryGetValue(uri + headerString, out result))
            {
                return result;
            }
            else
            {
                _logger.LogInformation($"Rquest URL {uri}");
                try
                {
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                        foreach (var header in headers ?? new Dictionary<string, string>())
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }

                        using (HttpResponseMessage response = await client.GetAsync(uri))
                        {

                            string responseBody = await response.Content.ReadAsStringAsync();

                            var responseResult = JsonConvert.DeserializeObject<T>(responseBody);

                            if (isCashe == true)
                            {

                                _memoryCache.Set(uri + headerString, responseResult, DateTimeOffset.UtcNow.AddMinutes(CacheTime()));

                            }
                            return responseResult;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw new DomainException(HttpStatusCode.BadGateway, ex.Message);
                }
            }
        }

        public uint CacheTime()
        {
            return UInt32.TryParse(_configuration.GetSection("CacheTime:Minutes").Value, out var tempVal) ? tempVal : 15;
        }
    }
}

