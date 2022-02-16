using System;
using System.Net.Http;

namespace CloudDataProtection.App.Shared.Rest
{
    public class UnsafeHttpClientProvider
    {
        private static UnsafeHttpClientProvider _instance;
        public static UnsafeHttpClientProvider Instance => _instance ??= new UnsafeHttpClientProvider();

        private HttpClient _httpClient;
        public HttpClient Client
        {
            get
            {
                _httpClient = _httpClient ?? new HttpClient
                (
                    new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    },
                    false
                )
                {
                    BaseAddress = new Uri("https://10.0.2.2:5001")
                };

                return _httpClient;
            }
        }

        private UnsafeHttpClientProvider()
        {
        }
    }
}
