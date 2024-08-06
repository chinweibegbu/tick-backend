using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateTest.Domain.Entities.Base
{
    public class EntityBase : IEntityBase
    {
        [Column("ID")]
        [Key]
        public string Id { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual void SetNewId()
        {
            throw new NotImplementedException();
        }
    }
}
