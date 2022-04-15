using System;
using System.ComponentModel;
using CloudDataProtection.Linq.Models;

namespace CloudDataProtection.Linq.Parser
{
    internal static class OrderByExpressionParser<TSource>
    {
        private const string Ascending = "ASC";
        private const string Descending = "DESC";
        
        public static OrderByExpression<TSource> Parse(string input)
        {
            string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                throw new ArgumentException(nameof(input));
            }

            string propertyName = parts[0].Trim();

            string sortingDirection = parts[1].Trim().ToUpper();

            ListSortDirection direction = sortingDirection switch
            {
                Ascending => ListSortDirection.Ascending,
                Descending => ListSortDirection.Descending,
                _ => throw new ArgumentException(nameof(input))
            };

            return new OrderByExpression<TSource>(propertyName, direction);
        }
    }
}