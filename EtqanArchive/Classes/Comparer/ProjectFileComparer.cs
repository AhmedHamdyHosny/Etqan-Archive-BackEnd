using EtqanArchive.DataLayer.TableEntity;
using System;

namespace Classes.Comparer
{
    public class ProjectFileComparer : IEntityEqualityComparer<ProjectFile>
    {
        public bool Equals(ProjectFile x, ProjectFile y)
        {
            if (x == null && y == null)
                return true;

            return x.ProjectFileId == y.ProjectFileId;
        }

        public int GetHashCode(ProjectFile model)
        {
            return model.ProjectFileId.GetHashCode();
        }

        public bool IsChanged(ProjectFile oldEntity, ProjectFile newEntity)
        {
            return oldEntity.ProjectId != newEntity.ProjectId ||
                oldEntity.CategoryId != newEntity.CategoryId ||
                oldEntity.FileExtensionId != newEntity.FileExtensionId ||
                //oldEntity.ContentTypeId != newEntity.ContentTypeId ||
                oldEntity.FileName != newEntity.FileName ||
                oldEntity.FilePath != newEntity.FilePath ||
                oldEntity.ContentTitle != newEntity.ContentTitle ||
                oldEntity.ContentAltTitle != newEntity.ContentAltTitle ||
                oldEntity.ContentDescription != newEntity.ContentDescription ||
                oldEntity.ContentAltDescription != newEntity.ContentAltDescription ||
                oldEntity.KeyWords != newEntity.KeyWords;
        }

        public Func<ProjectFile, bool> GetEquailtyPredicate(ProjectFile entity)
        {
            return (x => x.ProjectFileId == entity.ProjectFileId);
        }
    }
}
