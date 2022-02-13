using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Seeder
{
    public class AdminSeederOptions
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}