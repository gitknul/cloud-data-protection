using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Functions.BackupDemo.Authentication;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Functions.BackupDemo.Triggers
{
    public static class FileDestinationTrigger
    {
        [FunctionName("FileDestination")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequest request, ILogger logger)
        {
            if (!request.HttpContext.IsAuthenticated())
            {
                return new UnauthorizedResult();
            }

            FileDestination[] destinations = Enum.GetValues(typeof(FileDestination)) as FileDestination[];

            IEnumerable<FileDestinationResultEntry> sources = destinations
                .Select(d => new FileDestinationResultEntry((int) d, d.GetDescription()))
                .OrderBy(d => d.Description);

            FileDestinationResult result = new FileDestinationResult
            {
                Sources = sources
            };

            return new OkObjectResult(result);
        }
    }
}