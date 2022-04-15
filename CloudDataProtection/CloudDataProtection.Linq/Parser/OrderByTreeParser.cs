using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CloudDataProtection.Linq.Models;

namespace CloudDataProtection.Linq.Parser
{
    internal class OrderByTreeParser<TSource>
    {
        private static OrderByTreeParser<TSource> _instance;
        public static OrderByTreeParser<TSource> Instance => _instance ??= new OrderByTreeParser<TSource>();

        private ICollection<PropertyInfo> _properties;
        private ICollection<PropertyInfo> _disallowedProperties;

        public OrderByTree<TSource> Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }

            if (_properties == null && _disallowedProperties == null)
            {
                InitializePropertyInfos();
            }

            string[] parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            IList<OrderByExpression<TSource>> expressions = parts
                .Select(OrderByExpressionParser<TSource>.Parse)
                .Select(expression =>
                {
                    if (_disallowedProperties.Any(pi => pi.Name == expression.PropertyName))
                    {
                        throw new ArgumentException($"Sorting on property {expression.PropertyName} has been disabled");
                    }
                    
                    expression.SetPropertyInfo(_properties.First(pi => pi.Name == expression.PropertyName));
                    
                    return expression;
                }).ToList();
            
            return new OrderByTree<TSource>(expressions);
        }

        private void InitializePropertyInfos()
        {
            _properties = new List<PropertyInfo>();
            _disallowedProperties = new List<PropertyInfo>();
            
            PropertyInfo[] properties = typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.GetCustomAttributes<DisableSortingAttribute>(true).Any())
                {
                    _disallowedProperties.Add(propertyInfo);
                }
                else
                {
                    _properties.Add(propertyInfo);
                }
            }
        }
    }}