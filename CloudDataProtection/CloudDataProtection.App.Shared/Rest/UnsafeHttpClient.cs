using System;
using System.Net.Http;
using Xamarin.Essentials;

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
                    BaseAddress = DeviceInfo.Platform == DevicePlatform.Android 
                        ? new Uri("https://10.0.2.2:5001") 
                        : new Uri("https://127.0.0.1:5001")
                };
                
                return _httpClient;
            }
        }

        private UnsafeHttpClientProvider()
        {
        }
    }
}
