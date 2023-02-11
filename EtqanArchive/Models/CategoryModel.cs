using EtqanArchive.DataLayer.TableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;

namespace EtqanArchive.BackEnd.Models
{
    public class CategoryModel<TModel> : BaseModel<Category, Category, CategoryDAL>
    {
        public IEnumerable<TModel> GetData(Guid? CategoryId = null, IEnumerable<Guid> CategoryId_lst = null,
       bool? IsBlock = false, bool? IsDeleted = false, bool fromView = false,
        string IncludeProperties = null, string IncludeReferences = null, bool OrderByDate = false,
        GenericDataFormat requestBody = null, List<GenericDataFormat.FilterItem> filters = null)
        {
            if (filters == null)
            {
                filters = new List<GenericDataFormat.FilterItem>();
            }

            if (CategoryId != null)
            {
                filters.Add(CoreUtility.GetFilter(key: "CategoryId", value: CategoryId));
            }

            if (CategoryId_lst != null && CategoryId_lst.Any())
            {
                filters.AddRange(CoreUtility.GetFilter(key: "CategoryId", values: CategoryId_lst.Select(x => (object)x), LogicalOperation: "Or"));
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


    public class CategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryAltName { get; set; }
    }

    public class CategoryDetailsViewModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryGridViewModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryCreateBindModel : ICategory
    {
        public string CategoryName { get; set; }
    }

    public class CategoryEditBindModel : ICategory
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class Category_Create_Edit_Model
    {
        public ModelReference References { get; set; }

        public Category_Create_Edit_Model()
        {
            References = new ModelReference();
        }

        public class ModelReference
        {
        }


    }

    public class CategoryCreateModel : Category_Create_Edit_Model
    {
        public CategoryCreateBindModel Item { get; set; }
      
    }

    public class CategoryEditModel : Category_Create_Edit_Model
    {
        public CategoryEditBindModel Item { get; set; }
      
    }




    #region DAL
    public class CategoryDAL : BaseEntityDAL<Category, Category, CategoryDAL>
    {
    }

    #endregion
}
