using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer;
using EtqanArchive.DataLayer.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtqanArchive.BackEnd.Services
{
    public interface IContentTypeService
    {
        //bool PreEdit(Guid id, Guid userId, ContentTypeEditRequestModel model, ref ContentType entity);
        bool PreEdit(Guid id, Guid userId, ContentTypeEditRequestModel model, ref ContentType entity, ref JsonResponse<bool?> response);
        ContentType Edit(ContentType entity);
    }
    public class ContentTypeService : IContentTypeService
    {
        private readonly IMapper _mapper;

        public ContentTypeService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ContentType Edit(ContentType entity)
        {
            using (var context = new EtqanArchiveDBContext())
            {
                var oldEntity = context.ContentTypes.Include(j => j.FileExtensions)
                    .Single(j => j.ContentTypeId == entity.ContentTypeId && !j.IsBlock && !j.IsDeleted);

                // Update references
                // Update EmployeeBankAccount
                if (entity.FileExtensions != null)
                {
                    foreach (var childItem in entity.FileExtensions)
                    {
                        var oldFileExtension = oldEntity.FileExtensions.SingleOrDefault(c => c.FileExtensionId == childItem.FileExtensionId);
                        // Is original child item with same ID in DB?
                        if (oldFileExtension != null)
                        {
                            context.Entry(oldFileExtension).CurrentValues.SetValues(childItem);
                        }
                        else
                        {
                            context.FileExtensions.Add(childItem);
                        }
                    }

                    // Don't consider the child items we have just added above.
                    // (We need to make a copy of the list by using .ToList() because
                    // _dbContext.ChildItems.Remove in this loop does not only delete
                    // from the context but also from the child collection. Without making
                    // the copy we would modify the collection we are just interating
                    // through - which is forbidden and would lead to an exception.)
                    foreach (var oldChildItem in oldEntity.FileExtensions)
                    {
                        // Are there child items in the DB which are NOT in the
                        // new child item collection anymore?
                        if (!entity.FileExtensions.Any(c => c.FileExtensionId == oldChildItem.FileExtensionId))
                            // Yes -> It's a deleted child item -> Delete
                            context.FileExtensions.Remove(oldChildItem);
                    }
                }

                //entity.FileExtensions = null;
                // Update scalar/complex properties
                //context.Entry(oldEntity).CurrentValues.SetValues(entity);
                //context.Entry(oldEntity).State = EntityState.Detached;
                context.Entry(oldEntity).State = EntityState.Modified;
                context.SaveChanges();
                

            }

            return entity;
        }
        public bool PreEdit(Guid id, Guid userId, ContentTypeEditRequestModel model, ref ContentType entity, ref JsonResponse<bool?> response)
        {
            ContentType oldEntity = null;
            using (var context = new EtqanArchiveDBContext())
            {
               oldEntity = context.ContentTypes.Include(j => j.FileExtensions).SingleOrDefault(j => j.ContentTypeId == id && !j.IsBlock && !j.IsDeleted);
                if (oldEntity == null)
                {
                    response = new JsonResponse<bool?>(null, "Content Type not found", false);
                    return false;
                }

                if (model.FileExtensions != null && model.FileExtensions.Any())
                {
                    List<FileExtension> fileExtensions = new List<FileExtension>();
                    //set create user, create date for new items
                    foreach (var item in model.FileExtensions.Where(x => x.FileExtensionId == Guid.Empty))
                    {
                        FileExtension fileExtension = _mapper.Map<FileExtension>(item);
                        fileExtension.FileExtensionId = Guid.NewGuid();
                        fileExtension.ContentTypeId = entity.ContentTypeId;
                        fileExtension.CreateDate = DateTime.Now;
                        fileExtension.CreateUserId = userId;
                        fileExtensions.Add(fileExtension);
                    }

                    //loop over items that exist in old model items (search by id)
                    foreach (var item in model.FileExtensions.Where(x => oldEntity.FileExtensions.Any(y => y.FileExtensionId == x.FileExtensionId)))
                    {
                        //get old item
                        var fe = oldEntity.FileExtensions.SingleOrDefault(x => x.FileExtensionId == item.FileExtensionId);
                        FileExtension fileExtension = CoreUtility.CopyEntity(fe);

                        var originFileExtension = context.FileExtensions.SingleOrDefault(x => x.FileExtensionId == item.FileExtensionId);
                        //map updated properties values
                        //fileExtension = _mapper.Map<FileExtension>(item);
                        fileExtension.FileExtensionName = item.FileExtensionName;
                        fileExtension.ContentType = null;
                        //check item if modified from latest time
                        if (Repository<FileExtension>.IsChanged(
                            fileExtension, originFileExtension, context, GenericRepositoryCoreConstant.UpdateReference_ExcludedProperties))
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

            return true;
        }


        //public bool PreEdit(Guid id, Guid userId, ContentTypeEditRequestModel model, ref ContentType entity)
        //{
        //    ContentTypeModel<ContentType> contentTypeModel = new ContentTypeModel<ContentType>();
        //    ContentType oldEntity = null;
        //    bool isValid = PreEditValidation(entity, ref oldEntity);
        //    //ContentType oldEntity = contentTypeModel.GetData(ContentTypeId: entity.ContentTypeId, IncludeReferences: "FileExtensions").SingleOrDefault();
        //    if (model.FileExtensions != null && model.FileExtensions.Any())
        //    {
        //        List<FileExtension> fileExtensions = new List<FileExtension>();
        //        //set create user, create date for new items
        //        foreach (var item in model.FileExtensions.Where(x => x.FileExtensionId == Guid.Empty))
        //        {
        //            FileExtension fileExtension = _mapper.Map<FileExtension>(item);
        //            fileExtension.FileExtensionId = Guid.NewGuid();
        //            fileExtension.ContentTypeId = id;
        //            fileExtension.CreateDate = DateTime.Now;
        //            fileExtension.CreateUserId = userId;
        //            fileExtensions.Add(fileExtension);
        //        }

        //        //loop over items that exist in old model items (search by id)
        //        foreach (var item in model.FileExtensions.Where(x => oldEntity.FileExtensions.Any(y =>
        //        y.FileExtensionId.ToString() == x.FileExtensionId.ToString())))
        //        {
        //            List<FileExtension> oldFileExtensions = new List<FileExtension>(oldEntity.FileExtensions.ToList());
        //            //get old item
        //            var fileExtension = oldFileExtensions.SingleOrDefault(x =>
        //            x.FileExtensionId.ToString() == item.FileExtensionId.ToString());
        //            var originFileExtension = oldFileExtensions.SingleOrDefault(x =>
        //            x.FileExtensionId.ToString() == item.FileExtensionId.ToString());
        //            //map updated properties values
        //            //fileExtension = _mapper.Map<FileExtension>(item);
        //            fileExtension.FileExtensionName = item.FileExtensionName;
        //            fileExtension.ContentType = null;
        //            //check item if modified from latest time
        //            if (Repository<FileExtension>.IsChanged(
        //                fileExtension, originFileExtension, contentTypeModel.dbContext, GenericRepositoryCoreConstant.UpdateReference_ExcludedProperties))
        //            {
        //                //set modify user, modify date for updated items
        //                fileExtension.ModifyDate = DateTime.Now;
        //                fileExtension.ModifyUserId = userId;
        //            }
        //            fileExtensions.Add(fileExtension);
        //        }
        //        entity.FileExtensions = fileExtensions;
        //    }

        //    contentTypeModel.dbContext.Dispose();

        //    return true;
        //}

        //private bool PreEditValidation(ContentType entity, ref ContentType oldEntity)
        //{
        //    IEnumerable<ContentType> oldEntities = new ContentTypeModel<ContentType>().GetData(ContentTypeId: entity.ContentTypeId, IncludeReferences: "FileExtensions");
        //    if (oldEntities == null || !oldEntities.Any() || oldEntities.SingleOrDefault() == null)
        //    {
        //        return false;
        //    }
        //    oldEntity = oldEntities.SingleOrDefault();
        //    return true;
        //}
    }
}
