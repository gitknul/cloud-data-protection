using System;
using System.Collections.Generic;
using System.Linq;
using CloudDataProtection.Linq.Extensions;
using CloudDataProtection.Linq.Tests.Mocks;
using Xunit;

namespace CloudDataProtection.Linq.Tests
{
    public class EnumerableExtensionsTests
    {
        private readonly PersonProvider _personProvider;
        
        public EnumerableExtensionsTests()
        {
            _personProvider = new PersonProvider();
        }
        
        [Fact]
        public void TestOrderBySingleStringProperty()
        {
            string query = $"{nameof(Person.FirstName)} DESC";

            List<Person> personsDynamic = _personProvider.Persons.OrderBy(query).ToList();
            List<Person> personsLinq = _personProvider.Persons.OrderByDescending(p => p.FirstName).ToList();

            for (int i = 0; i < personsDynamic.Count; i++)
            {
                Assert.Equal(personsLinq[i].Id, personsDynamic[i].Id);
            }
        }
        
        [Fact]
        public void TestOrderBySingleIntProperty()
        {
            string query = $"{nameof(Person.Length)} DESC";

            List<Person> personsDynamic = _personProvider.Persons.OrderBy(query).ToList();
            List<Person> personsLinq = _personProvider.Persons.OrderByDescending(p => p.Length).ToList();

            for (int i = 0; i < personsDynamic.Count; i++)
            {
                Assert.Equal(personsLinq[i].Length, personsDynamic[i].Length);
            }
        }
        
        [Fact]
        public void TestOrderBySingleLongProperty()
        {
            string query = $"{nameof(Person.LongLength)} DESC";

            List<Person> personsDynamic = _personProvider.Persons.OrderBy(query).ToList();
            List<Person> personsLinq = _personProvider.Persons.OrderByDescending(p => p.LongLength).ToList();

            for (int i = 0; i < personsDynamic.Count; i++)
            {
                Assert.Equal(personsLinq[i].LongLength, personsDynamic[i].LongLength);
            }
        }
        
        [Fact]
        public void TestOrderBySingleDoubleProperty()
        {
            string query = $"{nameof(Person.AverageSpeed)} DESC";

            List<Person> personsDynamic = _personProvider.Persons.OrderBy(query).ToList();
            List<Person> personsLinq = _personProvider.Persons.OrderByDescending(p => p.AverageSpeed).ToList();

            for (int i = 0; i < personsDynamic.Count; i++)
            {
                Assert.Equal(personsLinq[i].AverageSpeed, personsDynamic[i].AverageSpeed);
            }
        }
        
        [Fact]
        public void TestOrderBySingleDecimalProperty()
        {
            string query = $"{nameof(Person.AverageScore)} DESC";

            List<Person> personsDynamic = _personProvider.Persons.OrderBy(query).ToList();
            List<Person> personsLinq = _personProvider.Persons.OrderByDescending(p => p.AverageScore).ToList();

            for (int i = 0; i < personsDynamic.Count; i++)
            {
                Assert.Equal(personsLinq[i].AverageScore, personsDynamic[i].AverageScore);
            }
        }
        
        [Fact]
        public void TestOrderByMultiplePropertiesSameOrder()
        {
            string query = $"{nameof(Person.FirstName)} DESC, {nameof(Person.LastName)} DESC";

            List<Person> personsDynamic = _personProvider.Persons.OrderBy(query).ToList();
            List<Person> personsLinq = _personProvider.Persons
                .OrderByDescending(p => p.FirstName)
                .ThenByDescending(p => p.LastName)
                .ToList();

            for (int i = 0; i < personsDynamic.Count; i++)
            {
                Person dynamic = personsDynamic[i];
                Person linq = personsLinq[i];
                
                Assert.Equal(linq.Id, dynamic.Id);
            }
        }

        [Fact]
        public void TestOrderByDisabledPropertyThrowsArgumentException()
        {
            string query = $"{nameof(Person.Password)} DESC";

            Assert.Throws<ArgumentException>(() => _personProvider.Persons.OrderBy(query));
        }

        [Fact]
        public void TestOrderByValidAndDisabledPropertyThrowsArgumentException()
        {
            string query = $"{nameof(Person.FirstName)} DESC, {nameof(Person.Password)} DESC";

            Assert.Throws<ArgumentException>(() => _personProvider.Persons.OrderBy(query));
        }

        [Fact]
        public void TestOrderByNullDoesNotThrow()
        {
            string query = null;
            
            _personProvider.Persons.OrderBy(query);
        }

        [Fact]
        public void TestOrderByEmptyStringDoesNotThrow()
        {
            string query = "";
            
            _personProvider.Persons.OrderBy(query);
        }

        [Fact]
        public void TestOrderByWhiteSpaceThrowsArgumentException()
        {
            string query = "  ";

            Assert.Throws<ArgumentException>(() => _personProvider.Persons.OrderBy(query));
        }
    }
}