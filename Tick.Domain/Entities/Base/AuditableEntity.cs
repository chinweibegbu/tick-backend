using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tick.Domain.Entities.Base
{
    public class AuditableEntity : EntityBase, IAuditableEntity
    {
        [Column("CREATED_BY")]
        public string CreatedBy { get; set; }
        [Column("UPDATED_BY")]
        public string UpdatedBy { get; set; }
        [Column("UPDATED_AT")]
        public DateTime? UpdatedAt { get; set; }
    }
}
