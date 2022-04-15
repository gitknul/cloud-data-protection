using System.Collections.Generic;

namespace CloudDataProtection.Core.Result
{
    public class PaginatedQueryResult<T>
    {
        public ICollection<T> Items { get; set; }
        public int ItemCount { get; set; }

        public PaginatedQueryResult(ICollection<T> items, int itemCount)
        {
            Items = items;
            ItemCount = itemCount;
        }
    }
}