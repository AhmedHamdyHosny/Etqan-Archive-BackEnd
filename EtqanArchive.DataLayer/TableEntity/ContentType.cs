using DataLayer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EtqanArchive.DataLayer.TableEntity
{
    public interface IContentType
    {
        public string ContentTypeName { get; set; }
        public string ContentTypeAltName { get; set; }
    }

    [Table("ContentType")]
    public class ContentType : CommonEntity, IContentType
    {
        public Guid ContentTypeId { get; set; }

        [StringLength(50), Required]
        public string ContentTypeName { get; set; }

        [StringLength(50)]
        public string ContentTypeAltName { get; set; }

        public virtual ICollection<FileExtension> FileExtensions { get; set; }

    }
}
