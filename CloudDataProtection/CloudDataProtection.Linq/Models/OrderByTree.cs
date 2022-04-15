using System.Collections.Generic;

namespace CloudDataProtection.Linq.Models
{
    internal class OrderByTree<TSource>
    {
        public IList<OrderByExpression<TSource>> Expressions { get; }

        public OrderByTree(IList<OrderByExpression<TSource>> expressions)
        {
            Expressions = expressions;
        }
    }
}