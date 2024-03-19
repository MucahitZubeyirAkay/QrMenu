using Microsoft.EntityFrameworkCore;

namespace QrMenuApi.Services
{
    public class DeleteGenericService
    {
        private readonly DbContext _context;

        public DeleteGenericService(DbContext context)
        {
            _context = context;
        }

        public void UpdateRelatedEntitiesState<TEntity, TRelatedEntity>(int entityId, byte newStateId)
            where TEntity : class
            where TRelatedEntity : class
        {
            var entity = _context.Set<TEntity>().Find(entityId);
            if (entity == null)
            {
                return;
            }

            var relatedEntities = _context.Set<TRelatedEntity>()
                                          .Where(r => EF.Property<int>(r, $"{typeof(TEntity).Name}Id") == entityId);

            foreach (var relatedEntity in relatedEntities)
            {
                var stateProperty = relatedEntity.GetType().GetProperty("StateId");
                if (stateProperty != null)
                {
                    stateProperty.SetValue(relatedEntity, newStateId);
                }
            }

            _context.SaveChanges();
        }
    }
}
