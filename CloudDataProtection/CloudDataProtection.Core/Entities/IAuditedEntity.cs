using System;

namespace CloudDataProtection.Core.Entities
{
    public interface IAuditedEntity<TKey> : IEntity<TKey>
    {
        public long CreatedByUserId { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public void SetAuditingInfo(long createdByUserId);
    }
}