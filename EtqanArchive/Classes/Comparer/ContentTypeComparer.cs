using EtqanArchive.DataLayer.TableEntity;
using System;

namespace Classes.Comparer
{
    public class ContentTypeComparer : IEntityEqualityComparer<ContentType>
    {
        public bool Equals(ContentType x, ContentType y)
        {
            if (x == null && y == null)
                return true;

            return x.ContentTypeId == y.ContentTypeId;
        }

        public int GetHashCode(ContentType model)
        {
            return model.ContentTypeId.GetHashCode();
        }

        public bool IsChanged(ContentType oldEntity, ContentType newEntity)
        {
            return oldEntity.ContentTypeName != newEntity.ContentTypeName ||
                oldEntity.ContentTypeAltName != newEntity.ContentTypeAltName;
        }

        public Func<ContentType, bool> GetEquailtyPredicate(ContentType entity)
        {
            return (x => x.ContentTypeId == entity.ContentTypeId);
        }
    }
}
