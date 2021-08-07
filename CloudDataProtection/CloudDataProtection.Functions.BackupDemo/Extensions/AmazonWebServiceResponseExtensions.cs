using Amazon.Runtime;

namespace CloudDataProtection.Functions.BackupDemo.Extensions
{
    public static class AmazonWebServiceResponseExtensions
    {
        public static bool IsSuccessStatusCode(this AmazonWebServiceResponse response)
        {
            return ((int) response.HttpStatusCode).ToString().StartsWith("2");
        }
    }
}