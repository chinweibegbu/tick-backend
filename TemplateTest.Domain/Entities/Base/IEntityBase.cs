using System;

namespace TemplateTest.Domain.Entities.Base
{
    public interface IEntityBase
    {
        string Id { get; }
        DateTime CreatedAt { get; set; }
        void SetNewId();
    }
}
