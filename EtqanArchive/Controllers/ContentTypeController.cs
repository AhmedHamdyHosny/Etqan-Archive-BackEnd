using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;
using EtqanArchive.Identity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtqanArchive.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentTypeController : BaseApiController<ContentType, ContentType, ContentTypeGridViewModel, ContentTypeDetailsViewModel,
        ContentTypeCreateModel, ContentTypeCreateBindModel, ContentTypeCreateBindModel, bool?, bool?, ContentTypeEditModel,
        ContentTypeEditBindModel, ContentTypeEditBindModel, bool?, ContentType,
        ContentTypeModel<ContentType>, ContentTypeModel<ContentType>>
    {
        private readonly IMapper _mapper;

        public ContentTypeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override bool FuncPreGetGridView(ref GenericDataFormat options, ref JsonResponse<PaginationResult<ContentTypeGridViewModel>> response)
        {
            options.Includes = new GenericDataFormat.IncludeItems()
            {
                Properties = "ContentTypeId,ContentTypeName",
            };
            return base.FuncPreGetGridView(ref options, ref response);
        }

        public override bool FuncPreCreate(ref ContentTypeCreateBindModel model, ref ContentType entity, ref JsonResponse<bool?> response)
        {
            bool success = base.FuncPreCreate(ref model, ref entity, ref response);
            if (success)
            {
                entity.FileExtensions = model.FileExtensions.Select(x =>
                {
                    var fileExtension = _mapper.Map<FileExtension>(x);
                    fileExtension.CreateDate = DateTime.Now;
                    fileExtension.CreateUserId = Guid.Parse(User.Identity.GetUserId());
                    return fileExtension;
                }).ToList();
            }
            return success;
        }

        public override IActionResult FuncPostInitEditView(Guid id, ref ContentTypeEditModel model, Guid? notificationId, Guid? taskId, ref JsonResponse<ContentTypeEditModel> response)
        {
            var fileExtensions = new FileExtensionModel<FileExtension>().GetData(ContentTypeId: id, IsBlock: false, IsDeleted: false);
            model.Item.FileExtensions = _mapper.Map<IEnumerable<FileExtensionEditBindModel>>(fileExtensions);
            return base.FuncPostInitEditView(id, ref model, notificationId, taskId, ref response);
        }

        public override bool FuncPreEdit(Guid id, ref ContentTypeEditBindModel model, ref ContentType entity, ref JsonResponse<bool?> response)
        {
            IsEdit_WithReference = true;
            EntityReferences = "FileExtensions";
            bool success = base.FuncPreEdit(id, ref model, ref entity, ref response);
            if (success)
            {
                if (model.FileExtensions != null && model.FileExtensions.Any())
                {
                    Guid userId = Guid.Parse(User.Identity.GetUserId());
                    ContentTypeModel<ContentType> contentTypeModel = new ContentTypeModel<ContentType>();
                    ContentType oldEntity = contentTypeModel.GetData(ContentTypeId: entity.ContentTypeId, IncludeReferences: EntityReferences).SingleOrDefault();
                    List<FileExtension> fileExtensions = new List<FileExtension>();
                    //set create user, create date for new items
                    foreach (var item in model.FileExtensions.Where(x => x.FileExtensionId == Guid.Empty || x.FileExtensionId == null))
                    {
                        FileExtension fileExtension = _mapper.Map<FileExtension>(item);
                        fileExtension.FileExtensionId = Guid.NewGuid();
                        fileExtension.ContentTypeId = id;
                        fileExtension.CreateDate = DateTime.Now;
                        fileExtension.CreateUserId = userId;
                        fileExtensions.Add(fileExtension);
                    }

                    //loop over items that exist in old model items (search by id)
                    foreach (var item in model.FileExtensions.Where(x => oldEntity.FileExtensions.Any(y =>
                    y.FileExtensionId.ToString() == x.FileExtensionId.ToString())))
                    {
                        //get old item
                        var fileExtension = oldEntity.FileExtensions.SingleOrDefault(x =>
                        x.FileExtensionId.ToString() == item.FileExtensionId.ToString());
                        var originFileExtension = oldEntity.FileExtensions.SingleOrDefault(x =>
                        x.FileExtensionId.ToString() == item.FileExtensionId.ToString());
                        //map updated properties values
                        fileExtension = _mapper.Map(item, fileExtension);
                        fileExtension.ContentType = null;
                        //check item if modified from latest time
                        if (Repository<FileExtension>.IsChanged(
                            fileExtension, originFileExtension, contentTypeModel.dbContext, GenericRepositoryCoreConstant.UpdateReference_ExcludedProperties))
                        {
                            //set modify user, modify date for updated items
                            fileExtension.ModifyDate = DateTime.Now;
                            fileExtension.ModifyUserId = userId;
                        }
                        fileExtensions.Add(fileExtension);
                    }
                    entity.FileExtensions = fileExtensions;
                }
            }
            return success;
        }


        public override IActionResult FuncPostDetailsView(bool success, Guid id, ref ContentTypeDetailsViewModel model, Guid? notificationId, ref JsonResponse<ContentTypeDetailsViewModel> response)
        {
            if (success)
            {
                var fileExtensions = new FileExtensionModel<FileExtension>().GetData(ContentTypeId: id, IsBlock: false, IsDeleted: false);
                model.FileExtensions = _mapper.Map<IEnumerable<FileExtensionEditBindModel>>(fileExtensions);
            }
            return base.FuncPostDetailsView(success, id, ref model, notificationId, ref response);
        }


        #region Mapper

        public override PaginationResult<ContentTypeGridViewModel> FuncGetGridMapViewModel(PaginationResult<ContentType> model)
        {
            return new PaginationResult<ContentTypeGridViewModel>()
            {
                TotalItemsCount = model.TotalItemsCount,
                PageItems = _mapper.Map<IEnumerable<ContentTypeGridViewModel>>(model.PageItems)
            };
        }

        public override ContentTypeDetailsViewModel FuncPreDetailsMapViewModel(ContentType model)
        {
            return _mapper.Map<ContentTypeDetailsViewModel>(model);
        }


        public override ContentType FuncPreCreateMapModel(ContentTypeCreateBindModel model)
        {
            return _mapper.Map<ContentType>(model);
        }
        public override ContentType FuncPreEditMapModel(Guid id, ContentTypeEditBindModel model, ContentType entity)
        {
            entity = _mapper.Map(model, entity);
            return entity;
        }
        public override ContentTypeEditBindModel FuncPreInitEditViewMapEntity(ContentType entity)
        {
            return _mapper.Map<ContentTypeEditBindModel>(entity);
        }
        #endregion

    }
}
