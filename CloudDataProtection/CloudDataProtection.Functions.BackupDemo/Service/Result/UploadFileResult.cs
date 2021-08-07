namespace CloudDataProtection.Functions.BackupDemo.Service.Result
{
    public class UploadFileResult
    {
        public bool Success { get; }
        public string Id { get; }

        private UploadFileResult(bool success)
        {
            Success = success;
        }

        private UploadFileResult(bool success, string id)
        {
            Success = success;
            Id = id;
        }

        public static UploadFileResult Ok(string id)
        {
            return new UploadFileResult(true, id);
        }

        public static UploadFileResult Error()
        {
            return new UploadFileResult(false);
        }
    }
}