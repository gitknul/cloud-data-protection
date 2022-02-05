using System;
using System.Collections.Generic;

namespace CloudDataProtection.Functions.BackupDemo.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddIf<T>(this ICollection<T> collection, Func<T> resolveFunction, bool predicate)
        {
            if (predicate)
            {
                T resolved = resolveFunction.Invoke();
                
                collection.Add(resolved);
            }
        }
    }
}