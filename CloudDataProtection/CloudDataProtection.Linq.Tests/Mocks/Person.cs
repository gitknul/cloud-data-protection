using System;

namespace CloudDataProtection.Linq.Tests.Mocks
{
    public class Person
    {
        public Guid Id { get; } = Guid.NewGuid();
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public int Length { get; set; }

        public long LongLength => Length;
        
        public decimal AverageScore { get; set; }
        
        public double AverageSpeed { get; set; }
        
        [DisableSorting]
        public string Password { get; set; }
    }
}