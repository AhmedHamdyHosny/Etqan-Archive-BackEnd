using DataLayer.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EtqanArchive.DataLayer.ViewEntity
{
    [NotMapped]
    public class ProjectFileView : CommonViewEntityWithNote
    {
        public Guid ProjectFileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentTitle { get; set; }
        public string ContentAltTitle { get; set; }
        public string ContentDescription { get; set; }
        public string ContentAltDescription { get; set; }
        public string KeyWords { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAltName { get; set; }
        public Guid ContentTypeId { get; set; }
        public string ContentTypeName { get; set; }
        public string ContentTypeAltName { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryAltName { get; set; }
        public Guid FileExtensionId { get; set; }
        public string FileExtensionName { get; set; }
        public string FileExtensionAltName { get; set; }
        public DateTime ProductionDate { get; set; }

    }
}
