using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CloudDataProtection.Linq.Models
{
    internal class OrderByExpression<TSource>
    {
        private const string ParameterName = "x";

        private PropertyInfo _propertyInfo;
        
        public string PropertyName { get; }
        public ListSortDirection Direction { get; }

        public OrderByExpression(string propertyName, ListSortDirection direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }

        public Func<TSource, object> Compile()
        {
            return DoCreateExpression().Compile();
        }

        public void SetPropertyInfo(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        private Expression<Func<TSource, object>> DoCreateExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), ParameterName);

            Expression property = Expression.Property(parameter, _propertyInfo);
            
            Expression conversion = Expression.Convert(property, typeof(object));
             
            return Expression.Lambda<Func<TSource, object>>(conversion, parameter);
        }
    }
}
