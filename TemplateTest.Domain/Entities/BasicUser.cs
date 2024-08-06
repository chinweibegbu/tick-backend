using TemplateTest.Domain.Common;
using TemplateTest.Domain.Entities.Base;
using TemplateTest.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateTest.Domain.Entities
{
    public class BasicUser : AuditableEntity
    {
        public BasicUser()
        {
            SetNewId();
        }
        [Column("NAME")]
        [Required]
        public string Name { get; set; }
        [Column("API_KEY")]
        [Required]
        public string ApiKey { get; set; }
        [Column("STATUS")]
        [Required]
        public BasicAuthStatus Status { get; set; }

        public override void SetNewId()
        {
            Id = $"BSR_{CoreHelpers.CreateUlid(DateTimeOffset.Now)}";
        }
    }
}
