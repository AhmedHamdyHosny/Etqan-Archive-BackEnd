using DataLayer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EtqanArchive.DataLayer.TableEntity
{
    public interface ICategory
    {
        string CategoryName { get; set; }
    }

    [Table("Category")]
    public class Category : CommonEntity, ICategory
    {
        public Guid CategoryId { get; set; }

        [StringLength(50), Required]
        public string CategoryName { get; set; }

        [StringLength(50)]
        public string CategoryAltName { get; set; }

        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }

    }
}
