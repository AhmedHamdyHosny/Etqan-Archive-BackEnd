using EtqanArchive.DataLayer.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtqanArchive.BackEnd.Models
{
    public class FileExtensionModel<TModel> : BaseModel<FileExtension, FileExtension, FileExtensionDAL>
    {
        public IEnumerable<TModel> GetData(Guid? FileExtensionId = null, IEnumerable<Guid> FileExtensionId_lst = null,
        string FileExtensionName = null, Guid? ContentTypeId = null,
        bool? IsBlock = false, bool? IsDeleted = false, bool fromView = false,
        string IncludeProperties = null, string IncludeReferences = null, bool OrderByDate = false,
        GenericDataFormat requestBody = null, List<GenericDataFormat.FilterItem> filters = null)
        {
            if (filters == null)
            {
                filters = new List<GenericDataFormat.FilterItem>();
            }

            if (FileExtensionId != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "FileExtensionId", value: FileExtensionId));
            }
            if (FileExtensionName != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "FileExtensionName", value: FileExtensionName));
            }
            if (ContentTypeId != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "ContentTypeId", value: ContentTypeId));
            }
            if (FileExtensionId_lst != null && FileExtensionId_lst.Any())
            {
                filters.AddRange(CoreUtility.GetFilter(key: "FileExtensionId", values: FileExtensionId_lst.Select(x => (object)x), LogicalOperation: "Or"));
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


    public class FileExtensionCreateBindModel : IFileExtension
    {
        public string FileExtensionName { get; set; }
    }

    public class FileExtensionEditBindModel : IFileExtension
    {
        public Guid FileExtensionId { get; set; }
        public string FileExtensionName { get; set; }
    }

    //public class FileExtensionReferenceModel : IFileExtension
    //{
    //    public Guid FileExtensionId { get; set; }
    //    public string FileExtensionName { get; set; }
    //    public ContentTypeReferenceModel ContentType { get; set; }
    //}


    #region DAL
    public class FileExtensionDAL : BaseEntityDAL<FileExtension, FileExtension, FileExtensionDAL>
    {
    }

    #endregion
}
