using System;
using System.ComponentModel;
using CloudDataProtection.Linq.Models;
using CloudDataProtection.Linq.Parser;
using CloudDataProtection.Linq.Tests.Mocks;
using Xunit;

namespace CloudDataProtection.Linq.Tests
{
    public class OrderByExpressionParserTests
    {
        [Fact]
        public void TestParseAscending()
        {
            string query = $"{nameof(Person.FirstName)} ASC";
            
            OrderByExpression<Person> expression = OrderByExpressionParser<Person>.Parse(query);
            
            Assert.Equal(nameof(Person.FirstName), expression.PropertyName);
            Assert.Equal(ListSortDirection.Ascending, expression.Direction);
        }
        
        [Fact]
        public void TestParseDescending()
        {
            string query = $"{nameof(Person.FirstName)} DESC";
            
            OrderByExpression<Person> expression = OrderByExpressionParser<Person>.Parse(query);
            
            Assert.Equal(nameof(Person.FirstName), expression.PropertyName);
            Assert.Equal(ListSortDirection.Descending, expression.Direction);
        }
        
        [Fact]
        public void TestParseAscending_LowercaseDirection()
        {
            string query = $"{nameof(Person.FirstName)} asc";
            
            OrderByExpression<Person> expression = OrderByExpressionParser<Person>.Parse(query);
            
            Assert.Equal(nameof(Person.FirstName), expression.PropertyName);
            Assert.Equal(ListSortDirection.Ascending, expression.Direction);
        }
        
        [Fact]
        public void TestParseDescending_LowercaseDirection()
        {
            string query = $"{nameof(Person.FirstName)} desc";
            
            OrderByExpression<Person> expression = OrderByExpressionParser<Person>.Parse(query);
            
            Assert.Equal(nameof(Person.FirstName), expression.PropertyName);
            Assert.Equal(ListSortDirection.Descending, expression.Direction);
        }

        [Fact]
        public void TestParseExistingPropertyWithoutDirectionThrowsArgumentException()
        {
            string query = $"{nameof(Person.FirstName)}";

            Assert.Throws<ArgumentException>(() => OrderByExpressionParser<Person>.Parse(query));
        }

        [Fact]
        public void TestParseNonExistingPropertyWithoutDirectionThrowsArgumentException()
        {
            string query = "NonExistingProperty";

            Assert.Throws<ArgumentException>(() => OrderByExpressionParser<Person>.Parse(query));
        }

        [Fact]
        public void TestParseExistingPropertyWithInvalidDirectionThrowsArgumentException()
        {
            string query = $"{nameof(Person.FirstName)} ABCD";

            Assert.Throws<ArgumentException>(() => OrderByExpressionParser<Person>.Parse(query));
        }

        [Fact]
        public void TestParseNonExistingPropertyWithInvalidDirectionThrowsArgumentException()
        {
            string query = "NonExistingProperty ABCD";

            Assert.Throws<ArgumentException>(() => OrderByExpressionParser<Person>.Parse(query));
        }

        [Fact]
        public void TestParseDisabledPropertyDoesNotThrow()
        {
            string query = $"{nameof(Person.Password)} ASC";

            OrderByExpression<Person> expression = OrderByExpressionParser<Person>.Parse(query);
            
            Assert.Equal(nameof(Person.Password), expression.PropertyName);
            Assert.Equal(ListSortDirection.Ascending, expression.Direction);
        }
    }
}