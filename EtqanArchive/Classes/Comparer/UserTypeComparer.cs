using DataLayer.Security.TableEntity;
using System;

namespace Classes.Comparer
{
    public class UserTypeComparer : IEntityEqualityComparer<UserType>
    {
        public bool Equals(UserType x, UserType y)
        {
            if (x == null && y == null)
                return true;

            return x.UserTypeId == y.UserTypeId;
        }

        public int GetHashCode(UserType model)
        {
            return model.UserTypeId.GetHashCode();
        }

        public bool IsChanged(UserType oldEntity, UserType newEntity)
        {
            return oldEntity.UserTypeName != newEntity.UserTypeName ||
                oldEntity.UserTypeAltName != newEntity.UserTypeAltName;
        }

        public Func<UserType, bool> GetEquailtyPredicate(UserType entity)
        {
            return (x => x.UserTypeId == entity.UserTypeId);
        }
    }
}
