using DataLayer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EtqanArchive.DataLayer.TableEntity
{
    public interface IFileExtension
    {
        public string FileExtensionName { get; set; }
    }

    [Table("FileExtension")]
    public class FileExtension : CommonEntity, IFileExtension
    {
        public Guid FileExtensionId { get; set; }

        [StringLength(50), Required]
        public string FileExtensionName { get; set; }

        [StringLength(50)]
        public string FileExtensionAltName { get; set; }
        public Guid ContentTypeId { get; set; }


        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }
        public virtual ContentType ContentType { get; set; }

    }
}
