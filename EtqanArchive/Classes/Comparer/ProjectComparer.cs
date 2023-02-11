using EtqanArchive.DataLayer.TableEntity;
using System;

namespace Classes.Comparer
{
    public class ProjectComparer : IEntityEqualityComparer<Project>
    {
        public bool Equals(Project x, Project y)
        {
            if (x == null && y == null)
                return true;

            return x.ProjectId == y.ProjectId;
        }

        public int GetHashCode(Project model)
        {
            return model.ProjectId.GetHashCode();
        }

        public bool IsChanged(Project oldEntity, Project newEntity)
        {
            return oldEntity.ProjectName != newEntity.ProjectName ||
                oldEntity.ProjectAltName != newEntity.ProjectAltName;
        }

        public Func<Project, bool> GetEquailtyPredicate(Project entity)
        {
            return (x => x.ProjectId == entity.ProjectId);
        }
    }
}
