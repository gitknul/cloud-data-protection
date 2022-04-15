using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CloudDataProtection.Linq.Models;
using CloudDataProtection.Linq.Parser;

namespace CloudDataProtection.Linq.Extensions
{
    public static class EnumerableExtensions
    {
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> enumerable, string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return enumerable.OrderBy(p => p);
            }
            
            OrderByTreeParser<TSource> parser = OrderByTreeParser<TSource>.Instance;

            OrderByTree<TSource> tree = parser.Parse(orderBy);

            OrderByExpression<TSource> expression = tree.Expressions[0];
            
            IOrderedEnumerable<TSource> orderedEnumerable = expression.Direction == ListSortDirection.Ascending
                ? enumerable.OrderBy(expression.Compile())
                : enumerable.OrderByDescending(expression.Compile());
            
            for (int i = 1; i < tree.Expressions.Count; i++)
            {
                expression = tree.Expressions[i];
                    
                orderedEnumerable = expression.Direction == ListSortDirection.Ascending
                    ? orderedEnumerable.ThenBy(expression.Compile())
                    : orderedEnumerable.ThenByDescending(expression.Compile());
            }
            
            return orderedEnumerable;
        }
    }
}