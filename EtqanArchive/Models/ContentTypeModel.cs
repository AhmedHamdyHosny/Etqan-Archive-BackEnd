using EtqanArchive.DataLayer.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtqanArchive.BackEnd.Models
{
    public class ContentTypeModel<TModel> : BaseModel<ContentType, ContentType, ContentTypeDAL>
    {
        public IEnumerable<TModel> GetData(Guid? ContentTypeId = null, IEnumerable<Guid> ContentTypeId_lst = null,
       bool? IsBlock = false, bool? IsDeleted = false, bool fromView = false,
        string IncludeProperties = null, string IncludeReferences = null, bool OrderByDate = false,
        GenericDataFormat requestBody = null, List<GenericDataFormat.FilterItem> filters = null)
        {
            if (filters == null)
            {
                filters = new List<GenericDataFormat.FilterItem>();
            }

            if (ContentTypeId != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "ContentTypeId", value: ContentTypeId));
            }

            if (ContentTypeId_lst != null && ContentTypeId_lst.Any())
            {
                filters.AddRange(CoreUtility.GetFilter(key: "ContentTypeId", values: ContentTypeId_lst.Select(x => (object)x), LogicalOperation: "Or"));
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
            if (OrderByDate)
            {
                requestBody.Sorts = requestBody.Sorts ?? new List<GenericDataFormat.SortItems>();
                requestBody.Sorts.Add(new GenericDataFormat.SortItems() { Property = "CreateDate", SortType = GenericDataFormat.SortType.Asc });
            }
            if (requestBody.Filters == null)
            {
                requestBody.Filters = new List<GenericDataFormat.FilterItem>(filters);
            }
            else
            {
                requestBody.Filters.InsertRange(0, filters);
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


    public class ContentTypeViewModel
    {
        public Guid ContentTypeId { get; set; }
        public string ContentTypeName { get; set; }
        public string ContentTypeAltName { get; set; }
    }

    public class ContentTypeDetailsViewModel
    {
        public Guid ContentTypeId { get; set; }
        public string ContentTypeName { get; set; }
        public virtual IEnumerable<FileExtensionEditBindModel> FileExtensions { get; set; }
    }

    public class ContentTypeGridViewModel
    {
        public Guid ContentTypeId { get; set; }
        public string ContentTypeName { get; set; }
    }

    public class ContentTypeCreateBindModel : IContentType
    {
        public string ContentTypeName { get; set; }
        public string ContentTypeAltName { get; set; }
        public virtual IEnumerable<FileExtensionCreateBindModel> FileExtensions { get; set; }
    }

    public class ContentTypeEditBindModel : IContentType
    {
        public string ContentTypeName { get; set; }
        public string ContentTypeAltName { get; set; }
        public virtual IEnumerable<FileExtensionEditBindModel> FileExtensions { get; set; }
    }

    public class ContentTypeEditRequestModel : IContentType
    {
        public string ContentTypeName { get; set; }
        public string ContentTypeAltName { get; set; }

        public virtual IEnumerable<FileExtensionEditBindModel> FileExtensions { get; set; }
    }

    public class ContentType_Create_Edit_Model
    {
        public ModelReference References { get; set; }

        public ContentType_Create_Edit_Model()
        {
            References = new ModelReference();
        }

        public class ModelReference
        {
        }


    }

    public class ContentTypeReferenceModel
    {
        public Guid ContentTypeId { get; set; }
        public string ContentTypeName { get; set; }
    }

    public class ContentTypeCreateModel : ContentType_Create_Edit_Model
    {
        public ContentTypeCreateBindModel Item { get; set; }

    }

    public class ContentTypeEditModel : ContentType_Create_Edit_Model
    {
        public ContentTypeEditBindModel Item { get; set; }

    }




    #region DAL
    public class ContentTypeDAL : BaseEntityDAL<ContentType, ContentType, ContentTypeDAL>
    {
    }

    #endregion
}
