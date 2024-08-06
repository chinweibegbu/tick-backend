using System;

namespace TemplateTest.Domain.Entities.Base
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        string UpdatedBy { get; set; }
    }
}
