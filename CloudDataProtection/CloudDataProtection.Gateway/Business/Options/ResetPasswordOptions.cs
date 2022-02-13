namespace CloudDataProtection.Business.Options
{
    public class ResetPasswordOptions
    {
        public int ExpiresInMinutes { get; set; }
        public string Url { get; set; }

        public string FormatUrl(string token) => string.Format(Url, token);
    }
}