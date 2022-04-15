namespace CloudDataProtection.Core.Controllers.Dto.Input
{
    public interface IPaginatedInput
    {
        int PageSize { get; set; }
        int Page { get; set; }
    }
}