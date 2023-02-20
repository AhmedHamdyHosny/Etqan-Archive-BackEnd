using EtqanArchive.DataLayer.TableEntity;
using EtqanArchive.DataLayer.ViewEntity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtqanArchive.BackEnd.Models
{
    public class ProjectFileModel<TModel> : BaseModel<ProjectFile, ProjectFileView, ProjectFileDAL>
    {
        public IEnumerable<TModel> GetData(Guid? ProjectFileId = null, IEnumerable<Guid> ProjectFileId_lst = null,
            Guid? ProjectId = null,
          bool? IsBlock = false, bool? IsDeleted = false, bool fromView = false, string IncludeProperties = null, string IncludeReferences = null,
          GenericDataFormat requestBody = null)
        {
            List<GenericDataFormat.FilterItem> filters = new List<GenericDataFormat.FilterItem>();
            if (ProjectFileId != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "ProjectFileId", value: ProjectFileId));
            }
            if (ProjectFileId_lst != null && ProjectFileId_lst.Any())
            {
                filters.AddRange(CoreUtility.GetFilter(key: "ProjectFileId", values: ProjectFileId_lst.Select(x => (object)x), LogicalOperation: "Or"));
            }
            if (ProjectId != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "ProjectId", value: ProjectId));
            }
            if (IsBlock != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "IsBlock", value: IsBlock));
            }
            if (IsDeleted != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "IsDeleted", value: IsDeleted));
            }
            if (requestBody == null)
            {
                requestBody = new GenericDataFormat();
            }
            if (IncludeProperties != null || IncludeReferences != null)
            {
                requestBody.Includes = new GenericDataFormat.IncludeItems() { Properties = IncludeProperties, References = IncludeReferences };
            }
            if (requestBody.Filters == null)
            {
                requestBody.Filters = new List<GenericDataFormat.FilterItem>(filters);
            }
            else
            {
                requestBody.Filters.AddRange(filters);
            }
            if (fromView)
            {
                return (IEnumerable<TModel>)GetView(requestBody);
            }
            else
            {
                return (IEnumerable<TModel>)Get(requestBody);
            }
        }
    }

    public class ProjectFileViewModel
    {
        public Guid ProjectFileId { get; set; }
        public string FileName { get; set; }
        public string FileExtensionName { get; set; }
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
        public DateTime? ProductionDate { get; set; }
        public virtual string Note { get; set; }
    }

    public class ProjectFileDetailsViewModel
    {
        public Guid ProjectFileId { get; set; }
        public string FileName { get; set; }
        public string FileExtensionName { get; set; }
        public string FilePath { get; set; }
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }
        public string KeyWords { get; set; }
        public string ProjectName { get; set; }
        public string ContentTypeName { get; set; }
        public string CategoryName { get; set; }
        public DateTime? ProductionDate { get; set; }
        public virtual string Note { get; set; }
    }


    public class ProjectFileGridViewModel
    {
        public Guid ProjectFileId { get; set; }
        public string FileName { get; set; }
        public string FileExtensionName { get; set; }
        public string ProjectName { get; set; }
        public string ContentTypeName { get; set; }
        public string CategoryName { get; set; }
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }
        public string FilePath { get; set; }
    }

    public class ProjectFileCreateBindModel : IProjectFile
    {
        public Guid FileExtensionId { get; set; }
        public Guid? CategoryId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }
        public string KeyWords { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string Note { get; set; }
        public double FileSize { get; set; }
        public int? Duration { get; set; }
    }

    public class ProjectFileEditBindModel : IProjectFile
    {
        public Guid ProjectFileId { get; set; }
        public Guid FileExtensionId { get; set; }
        public string ContentTypeName { get; set; }
        public Guid? CategoryId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }
        public string KeyWords { get; set; }
        public DateTime? ProductionDate { get; set; }
        public CategoryEditBindModel Category { get; set; }
        //public FileExtensionReferenceModel FileExtension { get; set; }
        public virtual string Note { get; set; }
        public double FileSize { get; set; }
        public string FormattedFileSize
        {
            get
            {
                if (FileSize < 1)
                {
                    //formate in KB
                    return $"{Math.Ceiling(FileSize * 1024)} KB";
                }
                else if (FileSize > 1024)
                {
                    //formate in GB
                    return $"{Math.Ceiling(FileSize / 1024)} GB";

                }
                else
                {
                    //formate in MB
                    return $"{Math.Ceiling(FileSize)} MB";
                }
            }
        }
        public int? Duration { get; set; }
        public string FormattedDuration
        {
            get
            {
                if (Duration != null && Duration > 0)
                {
                    TimeSpan t = TimeSpan.FromSeconds((double)Duration);
                    return string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds);
                }
                return null;
            }
        }
    }

    public class ProjectFileEditRequestModel : IProjectFile
    {
        public Guid ProjectFileId { get; set; }
        public Guid FileExtensionId { get; set; }
        public Guid? CategoryId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentTitle { get; set; }
        public string ContentDescription { get; set; }
        public string KeyWords { get; set; }
        public DateTime? ProductionDate { get; set; }
        public double FileSize { get; set; }
        public int? Duration { get; set; }
        public string Note { get; set; }
    }

    public class ProjectFile_Create_Edit_Model
    {
        public ModelReference References { get; set; }

        public ProjectFile_Create_Edit_Model()
        {
            References = new ModelReference();
        }

        public class ModelReference
        {
            public IEnumerable<CustomSelectListItem> ContentTypes { get; set; }
            public IEnumerable<CustomSelectListItem> Categories { get; set; }
            public IEnumerable<CustomSelectListItem> Projects { get; set; }
        }

        public void Create_Edit_SetReference()
        {
            References.ContentTypes = new ContentTypeModel<ContentType>()
                    .GetData(IsBlock: false, IsDeleted: false).Select(x => {
                        return new CustomSelectListItem()
                        {
                            Text = CoreUtility.GetDDLText(x.ContentTypeName, x.ContentTypeAltName),
                            AltText = CoreUtility.GetDDLAltText(x.ContentTypeName, x.ContentTypeAltName),
                            Value = x.ContentTypeId.ToString(),
                        };
                    });

            References.Categories = new CategoryModel<Category>()
                    .GetData(IsBlock: false, IsDeleted: false).Select(x => {
                        return new CustomSelectListItem()
                        {
                            Text = CoreUtility.GetDDLText(x.CategoryName, x.CategoryAltName),
                            AltText = CoreUtility.GetDDLAltText(x.CategoryName, x.CategoryAltName),
                            Value = x.CategoryId.ToString(),
                        };
                    });

            References.Projects = new ProjectModel<Project>()
                    .GetData(IsBlock: false, IsDeleted: false).Select(x => {
                        return new CustomSelectListItem()
                        {
                            Text = CoreUtility.GetDDLText(x.ProjectName, x.ProjectAltName),
                            AltText = CoreUtility.GetDDLAltText(x.ProjectName, x.ProjectAltName),
                            Value = x.ProjectId.ToString(),
                        };
                    });
        }
    }

    public class ProjectFileCreateModel : ProjectFile_Create_Edit_Model
    {
        public ProjectFileCreateBindModel Item { get; set; }
        public void Create_SetReference()
        {
            Create_Edit_SetReference();
        }
    }

    public class ProjectFileEditModel : ProjectFile_Create_Edit_Model
    {
        public ProjectFileEditBindModel Item { get; set; }
        public void Edit_SetReference()
        {
            Create_Edit_SetReference();
        }
    }

    public class ProjectFileGetSearchFilterRequestModel
    {
        public IEnumerable<CustomSelectListItem> ContentTypes { get; set; }
        public IEnumerable<CustomSelectListItem> Categories { get; set; }
        public IEnumerable<CustomSelectListItem> Projects { get; set; }
        public IEnumerable<CustomSelectListItem> FileExtensions { get; set; }
    }


    public class ProjectFileFileSearchRequestModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Search { get; set; }
        public string KeyWords { get; set; }
        public Guid? ContentTypeId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
        public IEnumerable<Guid> FileExtensionIds { get; set; }
    }

    public class DirecortyPathFilesRequestModel
    {
        public string DirectoryPath { get; set; }
    }

    public class DirecortyPathFilesResponseModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Guid FileExtensionId { get; set; }
        public string ContentTypeName { get; set; }
        public double FileSize { get; set; }
        public int? Duration { get; set; }
        public string FormattedFileSize 
        { 
            get
            {
                if(FileSize < 1)
                {
                    //formate in KB
                    return $"{Math.Ceiling(FileSize * 1024)} KB";
                }
                else if(FileSize > 1024)
                {
                    //formate in GB
                    return $"{Math.Ceiling(FileSize / 1024)} GB";

                }
                else
                {
                    //formate in MB
                    return $"{Math.Ceiling(FileSize)} MB";
                }
            }
        }
        public string FormattedDuration 
        { 
            get
            {
                if(Duration != null && Duration > 0)
                {
                    TimeSpan t = TimeSpan.FromSeconds((double)Duration);
                    return string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds);
                }
                return null;
            }
        }
    }


    


    #region DAL
    public class ProjectFileDAL : BaseEntityDAL<ProjectFile, ProjectFileView, ProjectFileViewDAL>
    {

    }

    public class ProjectFileViewDAL : BaseEntityDAL<ProjectFileView, ProjectFileView, ProjectFileViewDAL>
    {

    }

    #endregion
}
