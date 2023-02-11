using System;
using System.Collections.Generic;

namespace Classes.Comparer
{
    public interface IEntityEqualityComparer<TEntity> : IEqualityComparer<TEntity>
    {
        public bool IsChanged(TEntity oldEntity, TEntity newEntity);

        public Func<TEntity, bool> GetEquailtyPredicate(TEntity entity);
    }
}
