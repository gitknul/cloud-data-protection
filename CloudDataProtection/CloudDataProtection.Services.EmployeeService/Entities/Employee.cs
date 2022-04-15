using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Cryptography.Attributes;
using CloudDataProtection.Core.Entities;

namespace CloudDataProtection.Services.EmployeeService.Entities
{
    [Table("Employee")]
    public class Employee : IAuditedEntity<long>
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [Encrypt]
        public string FirstName { get; set; }
        
        [Required]
        [Encrypt]
        public string LastName { get; set; }
        
        [Required]
        public Gender Gender { get; set; }
        
        [Required]
        [Encrypt]
        public string ContactEmailAddress { get; set; }
        
        [Required]
        [Phone]
        [Encrypt]
        public string PhoneNumber { get; set; }
        
        public DateTime? ActivatedAt { get; set; }
        
        public EmployeeActivationStatus ActivationStatus { get; set; }
        
        public string ActivationStatusMessage { get; set; }
        
        public long? UserId { get; set; }

        public string UserEmailAddress { get; set; }

        #region Auditing

        [Required]
        public long CreatedByUserId { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }

        public void SetAuditingInfo(long createdByUserId)
        {
            CreatedAt = DateTime.Now;
            CreatedByUserId = createdByUserId;
        }

        #endregion

        [NotMapped] 
        public string FullName => string.Join(' ', FirstName, LastName);
    }
}