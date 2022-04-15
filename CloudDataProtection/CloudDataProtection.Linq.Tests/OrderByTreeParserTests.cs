using System;
using System.ComponentModel;
using System.Linq;
using CloudDataProtection.Linq.Models;
using CloudDataProtection.Linq.Parser;
using CloudDataProtection.Linq.Tests.Mocks;
using Xunit;

namespace CloudDataProtection.Linq.Tests
{
    public class OrderByTreeParserTests
    {
        private readonly OrderByTreeParser<Person> _parser;

        public OrderByTreeParserTests()
        {
            _parser = OrderByTreeParser<Person>.Instance;
        }
        
        [InlineData("FirstName ASC, LastName ASC")]
        [InlineData("LastName ASC, FirstName ASC")]
        [Theory]
        public void TestParseAscending(string propertyName)
        {
            OrderByTree<Person> tree = _parser.Parse(propertyName);
            
            Assert.Equal(2, tree.Expressions.Count);
            
            foreach (ListSortDirection direction in tree.Expressions.Select(e => e.Direction))
            {
                Assert.Equal(ListSortDirection.Ascending, direction);
            }
        }
        
        [InlineData("FirstName ASC, LastName DESC, Length DESC")]
        [Theory]
        public void TestParseAscendingMultipleDirections(string propertyName)
        {
            OrderByTree<Person> tree = _parser.Parse(propertyName);
            
            Assert.Equal(3, tree.Expressions.Count);
            
            Assert.Equal(ListSortDirection.Ascending, tree.Expressions[0].Direction);
            Assert.Equal(nameof(Person.FirstName), tree.Expressions[0].PropertyName);
            
            Assert.Equal(ListSortDirection.Descending, tree.Expressions[1].Direction);
            Assert.Equal(nameof(Person.LastName), tree.Expressions[1].PropertyName);
            
            Assert.Equal(ListSortDirection.Descending, tree.Expressions[2].Direction);
            Assert.Equal(nameof(Person.Length), tree.Expressions[2].PropertyName);
        }

        [Fact]
        public void TestNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.Parse(null));
        }

        [Fact]
        public void TestEmptyThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _parser.Parse(""));
        }

        [Fact]
        public void TestWhitespaceThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _parser.Parse(" "));
        }
    }
}