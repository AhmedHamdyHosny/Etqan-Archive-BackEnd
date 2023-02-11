using EtqanArchive.DataLayer.TableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classes.Comparer
{
    public class FileExtensionComparer : IEntityEqualityComparer<FileExtension>
    {
        public bool Equals(FileExtension x, FileExtension y)
        {
            if (x == null && y == null)
                return true;

            return x.FileExtensionId == y.FileExtensionId;
        }

        public int GetHashCode(FileExtension model)
        {
            return model.FileExtensionId.GetHashCode();
        }

        public bool IsChanged(FileExtension oldEntity, FileExtension newEntity)
        {
            return oldEntity.FileExtensionName != newEntity.FileExtensionName ||
                oldEntity.FileExtensionAltName != newEntity.FileExtensionAltName ||
                oldEntity.ContentTypeId != newEntity.ContentTypeId;
        }

        public Func<FileExtension, bool> GetEquailtyPredicate(FileExtension entity)
        {
            return (x => x.FileExtensionId == entity.FileExtensionId);
        }
    }
}
