using DataLayer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EtqanArchive.DataLayer.TableEntity
{
    public interface IProject
    {
        string ProjectName { get; set; }
        string ProjectLocation { get; set; }
    }

    [Table("Project")]
    public class Project : CommonEntity, IProject
    {
        public Guid ProjectId { get; set; }

        [StringLength(100), Required]
        public string ProjectName { get; set; }

        [StringLength(100)]
        public string ProjectAltName { get; set; }

        [StringLength(200)]
        public string ProjectLocation { get; set; }

        [StringLength(200)]
        public string ProjectAltLocation { get; set; }


        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }


    }
}
