using System;

namespace Tick.Domain.Entities.Base
{
    public interface IEntityBase
    {
        string Id { get; }
        DateTime CreatedAt { get; set; }
        void SetNewId();
    }
}
