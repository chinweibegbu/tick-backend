using Tick.Domain.Entities;
using Tick.Domain.Entities.Base;
using Tick.Domain.Enum;
using System.Threading.Tasks;

namespace Tick.Core.Contract.Repository
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        System.Threading.Tasks.Task InsertAuditLog<T>(T oldEntity, T newEntity, AuditEventType eventType, string userId) where T : AuditableEntity;
    }
}
