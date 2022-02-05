using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Core.Papertrail.Extensions
{
    public static class PapertrailLoggingBuilderExtensions
    {
        public static void ConfigureLogging(this ILoggingBuilder builder)
        {
            // Determine this by configuration
            if (Environment.Environment.CurrentEnvironment == Environment.Environment.Development)
            {
                builder = builder
                    .AddConsole()
                    .AddPapertrail();
            }
            else
            {
                builder = builder.ClearProviders();
                builder = builder.AddPapertrail();
            }
        }
    }
}