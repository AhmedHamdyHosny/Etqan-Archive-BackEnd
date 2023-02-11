using EtqanArchive.DataLayer.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtqanArchive.BackEnd.Models
{
    public class ProjectModel<TModel> : BaseModel<Project, Project, ProjectDAL>
    {
        public IEnumerable<TModel> GetData(Guid? ProjectId = null, IEnumerable<Guid> ProjectId_lst = null,
          
          bool? IsBlock = false, bool? IsDeleted = false, bool fromView = false, string IncludeProperties = null, string IncludeReferences = null,
          GenericDataFormat requestBody = null)
        {
            List<GenericDataFormat.FilterItem> filters = new List<GenericDataFormat.FilterItem>();
            if (ProjectId != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "ProjectId", value: ProjectId));
            }
            if (ProjectId_lst != null && ProjectId_lst.Any())
            {
                filters.AddRange(CoreUtility.GetFilter(key: "ProjectId", values: ProjectId_lst.Select(x => (object)x), LogicalOperation: "Or"));
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

    public class ProjectViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAltName { get; set; }
        public string ProjectLocation { get; set; }
        public string ProjectAltLocation { get; set; }
    }

    public class ProjectDetailsViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLocation { get; set; }
        public IEnumerable<ProjectFileEditBindModel> ProjectFiles { get; set; }
    }

    public class ProjectGridViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLocation { get; set; }
    }

    public class ProjectCreateBindModel : IProject
    {
        public string ProjectName { get; set; }
        public string ProjectLocation { get; set; }
        public IEnumerable<ProjectFileCreateBindModel> ProjectFiles { get; set; }
    }

    public class ProjectEditBindModel : IProject
    {
        public string ProjectName { get; set; }
        public string ProjectLocation { get; set; }
        public IEnumerable<ProjectFileEditBindModel> ProjectFiles { get; set; }
    }

    public class ProjectEditRequestModel : IProject
    {
        public string ProjectName { get; set; }
        public string ProjectLocation { get; set; }
        public IEnumerable<ProjectFileEditRequestModel> ProjectFiles { get; set; }
    }

    public class Project_Create_Edit_Model
    {
        public ModelReference References { get; set; }

        public Project_Create_Edit_Model()
        {
            References = new ModelReference();
        }

        public class ModelReference
        {
            public IEnumerable<CustomSelectListItem> Categories { get; set; }
            public IEnumerable<FileExtensionReferenceModel> FileExtensions { get; set; }
        }

        public void Create_Edit_SetReference()
        {
            References.Categories = new CategoryModel<Category>()
                    .GetData(IsBlock: false, IsDeleted: false).Select(x => {
                        return new CustomSelectListItem()
                        {
                            Text = CoreUtility.GetDDLText(x.CategoryName, x.CategoryAltName),
                            AltText = CoreUtility.GetDDLAltText(x.CategoryName, x.CategoryAltName),
                            Value = x.CategoryId.ToString(),
                        };
                    });

            References.FileExtensions = new FileExtensionModel<FileExtension>()
                    .GetData(IsBlock: false, IsDeleted: false, IncludeReferences: "ContentType").Select(x => {
                        return new FileExtensionReferenceModel()
                        {
                            FileExtensionId = x.FileExtensionId,
                            FileExtensionName = x.FileExtensionName,
                            ContentType = new ContentTypeReferenceModel()
                            {
                                ContentTypeId = x.ContentTypeId,
                                ContentTypeName =x.ContentType.ContentTypeName,
                            }
                        };
                    });
        }

    }

    public class ProjectCreateModel : Project_Create_Edit_Model
    {
        public ProjectCreateBindModel Item { get; set; }
        public void Create_SetReference()
        {
            Create_Edit_SetReference();
        }
    }

    public class ProjectEditModel : Project_Create_Edit_Model
    {
        public ProjectEditBindModel Item { get; set; }
        public void Edit_SetReference()
        {
            Create_Edit_SetReference();
        }
    }




    #region DAL
    public class ProjectDAL : BaseEntityDAL<Project, Project, ProjectDAL>
    {

    }

    #endregion
}
