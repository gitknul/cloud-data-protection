using System.Collections.Generic;

namespace CloudDataProtection.Core.Controllers.Dto.Output
{
    public class PaginatedOutput<TDto>
    {
        public IEnumerable<TDto> Data { get; set; }
        
        public int ItemCount { get; set; }
    }
}