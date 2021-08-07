using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Extensions
{
    public static class HttpRequestExtensions
    {
        public static T FromBody<T>(this HttpRequest request)
        {
            byte[] bytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                request.Body.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            
            string content = Encoding.UTF8.GetString(bytes);
            
            return JsonConvert.DeserializeObject<T>(content);
        }

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