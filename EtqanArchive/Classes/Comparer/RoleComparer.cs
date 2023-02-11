using System;
using DataLayer.Security.TableEntity;

namespace Classes.Comparer
{
    public class RoleComparer : IEntityEqualityComparer<Role>
    {
        public bool Equals(Role x, Role y)
        {
            if (x == null && y == null)
                return true;

            return x.Id == y.Id;
        }

        public int GetHashCode(Role model)
        {
            return model.Id.GetHashCode();
        }

        public bool IsChanged(Role oldEntity, Role newEntity)
        {
            return oldEntity.Name != newEntity.Name;
        }

        public Func<Role, bool> GetEquailtyPredicate(Role entity)
        {
            return (x => x.Id == entity.Id);
        }
    }
}
