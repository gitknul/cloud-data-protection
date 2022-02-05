using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Extensions
{
    public static class HttpRequestExtensions
    {
        public static T FromFormData<T>(this HttpRequest request, string key)
        {
            string content = request.Form[key];

            if (string.IsNullOrEmpty(content))
            {
                return default;
            }
            
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}