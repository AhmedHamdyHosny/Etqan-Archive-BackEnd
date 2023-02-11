using EtqanArchive.DataLayer.TableEntity;
using System;

namespace Classes.Comparer
{
    public class CategoryComparer : IEntityEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            if (x == null && y == null)
                return true;

            return x.CategoryId == y.CategoryId;
        }

        public int GetHashCode(Category model)
        {
            return model.CategoryId.GetHashCode();
        }

        public bool IsChanged(Category oldEntity, Category newEntity)
        {
            return oldEntity.CategoryName != newEntity.CategoryName ||
                oldEntity.CategoryAltName != newEntity.CategoryAltName;
        }

        public Func<Category, bool> GetEquailtyPredicate(Category entity)
        {
            return (x => x.CategoryId == entity.CategoryId);
        }
    }
}
