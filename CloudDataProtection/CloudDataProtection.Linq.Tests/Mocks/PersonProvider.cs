using System;
using System.Collections.Generic;

namespace CloudDataProtection.Linq.Tests.Mocks
{
    public class PersonProvider
    {
        private static readonly Random Random = new Random();

        private readonly ICollection<Person> _persons;
        public IEnumerable<Person> Persons => _persons;

        public PersonProvider()
        {
            _persons = new List<Person>();

            _persons.Add(new Person
            {
                FirstName = "John", 
                LastName = "Doe", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Jane", 
                LastName = "Doe", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Pete", 
                LastName = "Johnson", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Scott", 
                LastName = "Peterson", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Julian", 
                LastName = "Walker", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Patrick", 
                LastName = "James", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Kyle", 
                LastName = "James", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "James", 
                LastName = "Johnson", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Thom", 
                LastName = "Johnson", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Tim", 
                LastName = "Brown", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Rick", 
                LastName = "Williams", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
            _persons.Add(new Person
            {
                FirstName = "Robert", 
                LastName = "Johnson", 
                Length = Random.Next(160, 195), 
                AverageScore = (decimal)Random.NextDouble(), 
                AverageSpeed = Random.NextDouble() * 80,
                Password = "&%$!@%!"
            });
        }
    }
}