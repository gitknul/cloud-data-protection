using System;
using System.Threading.Tasks;
using System.Web.Http;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Authentication;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Factory;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Functions.BackupDemo.Triggers
{
    public static class FileDownloadTrigger
    {
        private static readonly string FileDownloadedFromHeader = "x-file-downloaded-from";
        private static readonly string FileNameHeader = "x-file-name";
        private static readonly string ContentDisposition = "content-disposition";

        private static readonly string[] ExposedHeaders = {FileDownloadedFromHeader, FileNameHeader, ContentDisposition};
        
        [FunctionName("FileDownload")]
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
            
            logger.LogInformation("Received request for file download with id {Id}", id);

            return await DoFileDownload(guid, request);
        }

        private static async Task<IActionResult> DoFileDownload(Guid id, HttpRequest request)
        {
            FileManagerLogic logic = FileManagerLogicFactory.Instance.GetLogic();

            BusinessResult<FileDownloadResult> result = await logic.Download(id);

            if (!result.Success || !result.Data.DownloadedFrom.HasValue)
            {
                return new InternalServerErrorResult();
            }
            
            request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", string.Join(", ", ExposedHeaders));
            request.HttpContext.Response.Headers.Add(FileDownloadedFromHeader, result.Data.DownloadedFrom.GetDescription());
            request.HttpContext.Response.Headers.Add(FileNameHeader, result.Data.FileName);

            return new FileContentResult(result.Data.Bytes, result.Data.ContentType)
            {
                FileDownloadName = result.Data.FileName
            };
        }
    }
}