using DataLayer.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EtqanArchive.DataLayer.TableEntity
{
    public interface IProjectFile
    {
        public Guid? CategoryId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }
        public string KeyWords { get; set; }
        public DateTime ProductionDate { get; set; }
        public Guid FileExtensionId { get; set; }
        public double FileSize { get; set; }
        public int? Duration { get; set; }
    }

    [Table("ProjectFile")]
    public class ProjectFile : CommonEntityWithNote, IProjectFile
    {
        public Guid ProjectFileId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid FileExtensionId { get; set; }

        [StringLength(100)]
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [StringLength(50)]
        public string ContentTitle { get; set; }
        [StringLength(50)]
        public string ContentAltTitle { get; set; }
        [StringLength(300)]
        public string ContentDescription { get; set; }
        [StringLength(300)]
        public string ContentAltDescription { get; set; }
        [StringLength(500)]
        public string KeyWords { get; set; }
        public DateTime ProductionDate { get; set; }
        [Column(TypeName = "float")]
        public double FileSize { get; set; }
        public int? Duration { get; set; }

        public virtual Project Project { get; set; }
        public virtual Category Category { get; set; }
        public virtual FileExtension FileExtension { get; set; }
    }
}
