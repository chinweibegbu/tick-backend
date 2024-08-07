using Tick.Domain.Common;
using Tick.Domain.Entities.Base;
using Tick.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tick.Domain.Entities
{
    public class AuditLog : EntityBase
    {
        public AuditLog()
        {
            SetNewId();
        }

        [Column("USER_ID")]
        [Required]
        public string UserId { get; set; }
        [Column("PROPERTY")]
        public string Property { get; set; }
        [Column("AUDIT_TYPE")]
        public AuditEventType EventType { get; set; }
        [Column("ORIGINAL_VALUE")]
        public string OriginalValue { get; set; }
        [Column("CURRENT_VALUE")]
        public string CurrentValue { get; set; }

        public override void SetNewId()
        {
            Id = $"AUD_{CoreHelpers.CreateUlid(DateTimeOffset.Now)}";
        }
    }
}
