using System.Linq;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;

namespace EnterpriseApp.API.Core.Extensions
{
    public static class ModelExtension
    {
        public static IQueryable<BaseEntity> FilterAllActive(this IQueryable<BaseEntity> queryable)
        {
            return queryable.Where(q => q.Status == (int)WellKnownStatus.Active);
        }

        public static bool IsActive(this BaseEntity entity)
        {
            return entity?.Status == (int)WellKnownStatus.Active;
        }
    }
}
