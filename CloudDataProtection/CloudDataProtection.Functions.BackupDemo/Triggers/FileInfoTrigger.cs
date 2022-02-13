using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Authentication;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Factory;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Functions.BackupDemo.Triggers
{
    public static class FileRetrieveTrigger
    {
        [FunctionName("FileInfo")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequest request, ILogger logger)
        {
            if (!request.HttpContext.IsAuthenticated())
            {
                return new UnauthorizedResult();
            }

            string id = request.Query["id"];
            
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out Guid guid))
            {
                return new BadRequestResult();
            }
            
            logger.LogInformation("Received request for file retrieval with id {Id}", id);

            return await DoGetFileInfo(guid);
        }

        private static async Task<IActionResult> DoGetFileInfo(Guid id)
        {
            FileManagerLogic logic = FileManagerLogicFactory.Instance.GetLogic();

            BusinessResult<File> result = await logic.Get(id);
            
            if (!result.Success)
            {
                if (result.ErrorType == ResultError.NotFound)
                {
                    return new NotFoundResult();
                }
                
                return new InternalServerErrorResult();
            }

            FileInfoOutput output = new()
            {
                Name = result.Data.DisplayName,
                Bytes = result.Data.Bytes,
                ContentType = result.Data.ContentType,
                UploadedTo = result.Data.UploadedTo
                    .Select(f => new FileInfoDestinationResultEntry(f))
                    .ToList()
            };
            
            return new OkObjectResult(output);
        }
    }
}