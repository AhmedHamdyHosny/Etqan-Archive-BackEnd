using GenericBackEndCore.Classes.Common;
using GenericBackEndCore.Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Tazkara.DataLayer.ViewEntity;
using EtqanArchive.Identity;
using GenericRepositoryCore.Utilities;
using static GenericRepositoryCore.Utilities.GenericDataFormat;
using Microsoft.AspNetCore.Mvc;
using EtqanArchive.Localization.Resources;

namespace EtqanArchive.BackEnd.Controllers
{
    [Authorize]
    public class BaseApiController<TDBEntity, TDBViewEntity, TGridViewModel, TDetailsViewModel, TCreateModel, TCreateItemModel, 
        TCreateRequestModel, TCreateResponseModel, TCreateGroupResponseModel, TEditModel, TEditItemModel, TEditRequestModel, TEditResponseModel,
        TImportModel, TModel_TDBEntity, TModel_TDBViewEntity> : GenericBaseApiController<TDBEntity, TDBViewEntity, TGridViewModel, TDetailsViewModel,
        TCreateModel, TCreateItemModel, TCreateRequestModel, TCreateResponseModel, TCreateGroupResponseModel, TEditModel, TEditItemModel, TEditRequestModel, 
        TEditResponseModel, TImportModel,TModel_TDBEntity, TModel_TDBViewEntity, Guid, UserView, Guid?, Guid?>
        where TDBEntity : class, new()
        where TDBViewEntity : class, new()
        //where TDetailsViewModel : TDBViewEntity, new()
        //where TCreateBindModel : TDBEntity, new()
        //where TEditBindModel : TDBEntity, new()
        where TDetailsViewModel : class, new()
        where TCreateRequestModel : new()
        //where TCreateResponseModel : class
        where TEditRequestModel : new()
        where TCreateModel : new()
        where TCreateItemModel : new()
        where TEditModel : new()
        where TEditItemModel : new()
        where TModel_TDBEntity : new()
        where TModel_TDBViewEntity : new()
    {

        public override DBEnums.AccessType<Guid> AccessType
        {
            get
            {
                
                return DBEnums.AccessType<Guid>.Instance;
            }
        }

        public override AlertMessageResource MessageResource
        {
            get
            {
                return AlertMessageResource.Instance;
            }
        }

        public override bool FuncPreGetGridView(ref GenericDataFormat options, ref JsonResponse<PaginationResult<TGridViewModel>> response)
        {
            options.Filters = options.Filters ?? new List<FilterItem>();
            //foreach (var filterItem in options.Filters)
            //{
            //    if (filterItem.Value is string)
            //    {
            //        Guid convertedGuid;
            //        bool vaildGuid = Guid.TryParse(filterItem.Value as string, out convertedGuid);
            //        if (vaildGuid)
            //        {
            //            filterItem.Value = convertedGuid;
            //        }
            //    }
            //}

            options.Filters.Add(new FilterItem()
            {
                Property = "IsDeleted",
                Value = false,
                Operation = FilterOperation.Equal,
                LogicalOperation = LogicalOperation.And
            });
            return base.FuncPreGetGridView(ref options, ref response);
        }

        public override bool FuncPreDetailsView(Guid id, ref TDetailsViewModel model, ref JsonResponse<TDetailsViewModel> response)
        {
            dynamic instance = Activator.CreateInstance(typeof(TModel_TDBViewEntity));
            TDBViewEntity viewEntity = instance.GetView(id);
            if (viewEntity == null)
            {
                response = new JsonResponse<TDetailsViewModel>(null, Resource.ErrorMsg_ItemNotFound, false);
                return false;
            }
            //model = CoreUtility.CopyObject<TDBViewEntity, TDetailsViewModel>(viewModel);
            model = FuncPreDetailsMapViewModel(viewEntity);
            return base.FuncPreDetailsView(id, ref model, ref response);
        }

        public override bool FuncPreCreate(ref TCreateRequestModel model, ref TDBEntity entity, ref JsonResponse<TCreateResponseModel> response)
        {
            if (model == null || !ModelState.IsValid)
            {
                response = new JsonResponse<TCreateResponseModel>(Validation.InvalidModel, false);
                return false;
            }
            //map create object
            entity = FuncPreCreateMapModel(model);
            //set primary key value
            dynamic instance = Activator.CreateInstance(typeof(TModel_TDBEntity));
            IEnumerable<string> keyNames = instance.GetPKColumns();
            if (keyNames != null && keyNames.SingleOrDefault() != null)
            {
                PropertyInfo pkProp = typeof(TDBEntity).GetProperty(keyNames.SingleOrDefault());
                pkProp.SetValue(entity, Guid.NewGuid());
            }
            //set create user value
            Type modelType = typeof(TDBEntity);
            PropertyInfo createUserId = modelType.GetProperty("CreateUserId");
            if (createUserId != null)
            {
                createUserId.SetValue(entity, Guid.Parse(User.Identity.GetUserId()));
            }
            //set create date value
            PropertyInfo createDate = modelType.GetProperty("CreateDate");
            if (createDate != null)
            {
                createDate.SetValue(entity, DateTimeOffset.Now);
            }
            return true;
        }

        public override bool FuncPreCreate(ref IEnumerable<TCreateRequestModel> models, ref IEnumerable<TDBEntity> entities, ref JsonResponse<TCreateGroupResponseModel> response)
        {
            //map create object
            entities = FuncPreCreateGroupMapModels(models);

            //get primary key column name
            dynamic instance = Activator.CreateInstance(typeof(TModel_TDBEntity));
            IEnumerable<string> keyNames = instance.GetPKColumns();
            //get type of model
            Type modelType = typeof(TCreateRequestModel);
            //get primary key propery
            PropertyInfo pkProp = (keyNames != null && keyNames.SingleOrDefault() != null) ?
                typeof(TCreateRequestModel).GetProperty(keyNames.SingleOrDefault()) : null;
            PropertyInfo createUserId = modelType.GetProperty("CreateUserId");
            PropertyInfo createDate = modelType.GetProperty("CreateDate");
            bool hasPrimayKey = (pkProp != null), hasCreateUserIdProperty = (createUserId != null), hasCreateDateProperty = (createDate != null);

            if (hasPrimayKey || hasCreateUserIdProperty || hasCreateDateProperty)
            {
                foreach (var item in models)
                {
                    if (hasPrimayKey)
                        pkProp.SetValue(item, Guid.Parse(User.Identity.GetUserId()));
                    if (hasCreateUserIdProperty)
                        createUserId.SetValue(item, SystemUser.UserId);
                    if (hasCreateDateProperty)
                        createDate.SetValue(item, DateTimeOffset.Now);
                }
            }
            return true;
        }

        public override IActionResult FuncPostCreate(bool success, TCreateRequestModel model, ref TDBEntity insertedItem, Guid? notificationId, Guid? taskId, ref JsonResponse<TCreateResponseModel> response)
        {
            if (typeof(TCreateResponseModel) == typeof(bool?) || typeof(TCreateResponseModel) == typeof(bool))
                response.Result = (dynamic)success;
            return base.FuncPostCreate(success, model, ref insertedItem, notificationId, taskId, ref response);
        }

        public override bool FuncPreEdit(Guid id, ref TEditRequestModel model, ref TDBEntity entity, ref JsonResponse<TEditResponseModel> response)
        {
            //check id
            if (id == null || id == Guid.Empty)
            {
                response = new JsonResponse<TEditResponseModel>(Resource.InvalidId, false);
                return false;
            }
                
            //check model
            if (model == null || !ModelState.IsValid)
            {
                response = new JsonResponse<TEditResponseModel>(Validation.InvalidModel, false);
                return false;
            }
            //chech if entity is exist
            dynamic instance = Activator.CreateInstance(typeof(TModel_TDBEntity));
            entity = instance.Get(id);
            if(entity == null)
            {
                response = new JsonResponse<TEditResponseModel>(Resource.ErrorMsg_ItemNotFound, false);
                return false;
            }

            entity = FuncPreEditMapModel(id, model, entity);
            Type modelType = typeof(TDBEntity);
            PropertyInfo modifyUserId = modelType.GetProperty("ModifyUserId");
            if (modifyUserId != null)
            {
                modifyUserId.SetValue(entity, Guid.Parse(User.Identity.GetUserId()));
            }
            PropertyInfo modifyDate = modelType.GetProperty("ModifyDate");
            if (modifyDate != null)
            {
                modifyDate.SetValue(entity, DateTimeOffset.Now);
            }
            return true;
        }

        public override IActionResult FuncPostEdit(bool success, ref TEditRequestModel model, ref TDBEntity updatedItem, Guid? notificationId, Guid? taskId, ref JsonResponse<TEditResponseModel> response)
        {
            if (typeof(TEditResponseModel) == typeof(bool?) || typeof(TEditResponseModel) == typeof(bool))
                response.Result = (dynamic)success;
            return base.FuncPostEdit(success, ref model, ref updatedItem, notificationId, taskId, ref response);
        }

        public override IEnumerable<FilterItem> GetUserServiceAccessConditionAsFilter(Guid serviceId, Guid accessTypeId)
        {
            return null;
        }

        public override void SetAccessPermission(ref IBaseAccessPermission userAccessPermission)
        {
            //throw new NotImplementedException();
        }

        public override bool UserHasPermission(Guid serviceId, Guid accessTypeId)
        {
            ////check if action allow Anonymous
            //var allowAnonymous = HttpContext.GetEndpoint().Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
            //if (allowAnonymous)
            //    return true;

            return true;
        }

        #region Mapping
        public override PaginationResult<TGridViewModel> FuncGetGridMapViewModel(PaginationResult<TDBViewEntity> model)
        {
            return null;
        }
        public override TDetailsViewModel FuncPreDetailsMapViewModel(TDBViewEntity model)
        {
            return new TDetailsViewModel();
        }

        public override TDBEntity FuncPreCreateMapModel(TCreateRequestModel model)
        {
            return new TDBEntity();
        }

        public override IEnumerable<TDBEntity> FuncPreCreateGroupMapModels(IEnumerable<TCreateRequestModel> models)
        {
            return new List<TDBEntity>();
        }

        public override TDBEntity FuncPreEditMapModel(Guid id, TEditRequestModel model, TDBEntity entity)
        {
            return entity;
        }

        public override TEditItemModel FuncPreInitEditViewMapEntity(TDBEntity entity)
        {
            return new TEditItemModel();
        }
        #endregion

        [Authorize(Roles = Classes.Common.DBEnums.Roles.Admin)]
        public override IActionResult Export([FromBody] GenericDataFormat options, [FromRoute] Guid? notificationId, [FromRoute] Guid? taskId, [FromRoute] string fileType = "Excel")
        {
            return null;
        }
    }
}
