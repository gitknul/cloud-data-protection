using System.Collections.Generic;

namespace CloudDataProtection.Core.Result
{
    public class PaginatedBusinessResult : PaginatedBusinessResult<object>
    {
    }

    public class PaginatedBusinessResult<T> : BusinessResult<ICollection<T>>
    {
        public int ItemCount { get; set; }

        public static PaginatedBusinessResult<T> Ok(ICollection<T> data, int itemCount)
        {
            return new PaginatedBusinessResult<T>
            {
                ItemCount = itemCount,
                Data = data,
                Success = true
            };
        }

        protected PaginatedBusinessResult()
        {
        }
    }
}