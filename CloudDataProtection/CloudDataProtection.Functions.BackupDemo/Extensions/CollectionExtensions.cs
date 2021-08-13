using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CloudDataProtection.Functions.BackupDemo.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddIf<T>(this ICollection<T> collection, Expression<Func<T>> resolveFunction, bool predicate)
        {
            if (predicate)
            {
                T resolved = resolveFunction.Compile().Invoke();
                
                collection.Add(resolved);
            }
        }
    }
}