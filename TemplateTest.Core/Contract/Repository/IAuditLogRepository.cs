using TemplateTest.Domain.Entities;
using TemplateTest.Domain.Entities.Base;
using TemplateTest.Domain.Enum;
using System.Threading.Tasks;

namespace TemplateTest.Core.Contract.Repository
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task InsertAuditLog<T>(T oldEntity, T newEntity, AuditEventType eventType, string userId) where T : AuditableEntity;
    }
}
