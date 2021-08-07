using System.IO;
using System.Threading.Tasks;

namespace CloudDataProtection.Functions.BackupDemo.Extensions
{
    public static class StreamExtensions
    {
        public static async Task CopyToAndSeekAsync(this Stream source, Stream destination)
        {
            source.Position = 0;
            
            await source.CopyToAsync(destination);

            destination.Position = 0;
        }
    }
}