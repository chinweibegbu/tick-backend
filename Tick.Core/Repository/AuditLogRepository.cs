using Tick.Core.Contract.Repository;
using Tick.Core.Repository.Base;
using Tick.Domain.Common;
using Tick.Domain.Entities;
using Tick.Domain.Entities.Base;
using Tick.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Tick.Core.Repository
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<AuditLog>())
        {
        }

        public async System.Threading.Tasks.Task InsertAuditLog<T>(T oldEntity, T newEntity, AuditEventType eventType, string userId) where T : AuditableEntity
        {
            string oldEntityJson = CoreHelpers.ClassToJsonData(oldEntity);
            string newEntityJson = CoreHelpers.ClassToJsonData(newEntity);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                var _entities = _getDbSet(dbContext);

                var auditLog = new AuditLog()
                {
                    UserId = userId,
                    Property = typeof(T).Name,
                    EventType = eventType,
                    OriginalValue = oldEntityJson,
                    CurrentValue = newEntityJson
                };

                await _entities.AddAsync(auditLog);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
