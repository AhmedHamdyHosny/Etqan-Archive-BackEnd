using AutoMapper;
using DataLayer.Common;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;
using EtqanArchive.DataLayer.ViewEntity;
using EtqanArchive.Localization.Resources;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtqanArchive.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectFileController : BaseApiController<ProjectFile, ProjectFileView, ProjectFileGridViewModel, ProjectFileDetailsViewModel,
        ProjectFileCreateModel, ProjectFileCreateBindModel, ProjectFileCreateBindModel, bool?, bool?, ProjectFileEditModel,
        ProjectFileEditBindModel, ProjectFileEditBindModel, bool?, ProjectFile,
        ProjectFileModel<ProjectFile>, ProjectFileModel<ProjectFileView>>
    {
        private readonly IMapper _mapper;

        public ProjectFileController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override bool FuncPreGetGridView(ref GenericDataFormat options, ref JsonResponse<PaginationResult<ProjectFileGridViewModel>> response)
        {
            options.Includes = new GenericDataFormat.IncludeItems()
            {
                Properties = "ProjectFileId,FileName,FilePath,FileExtensionName,ProjectName,ContentTypeName,CategoryName",
            };
            return base.FuncPreGetGridView(ref options, ref response);
        }

        //public override bool FuncPreCreate(ref ProjectFileCreateBindModel model, ref ProjectFile entity, ref JsonResponse<bool?> response)
        //{
        //    bool success = base.FuncPreCreate(ref model, ref entity, ref response);
        //    if (success)
        //    {
        //        if (model.FileExtensionName == null)
        //        {
        //            response = new JsonResponse<bool?>(Validation.InvalidModel, false);
        //            success = false;
        //        }
        //        else
        //        {
        //            var fileExtensionModel = new FileExtensionModel<FileExtension>();
        //            var fileExtension = fileExtensionModel.GetData(FileExtensionName: model.FileExtensionName, IsBlock: false, IsDeleted: false).SingleOrDefault();

        //            if (fileExtension == null)
        //            {
        //                var insertedExtension = fileExtensionModel.Insert(new FileExtension()
        //                {
        //                    FileExtensionId = Guid.NewGuid(),
        //                    FileExtensionName = model.FileExtensionName,
        //                    FileExtensionAltName = model.FileExtensionName,
        //                    CreateDate = DateTime.Now,
        //                    CreateUserId = Guid.Parse(User.Identity.GetUserId()),
        //                });

        //                entity.FileExtensionId = insertedExtension.FileExtensionId;
        //            }
        //            else
        //            {
        //                entity.FileExtensionId = fileExtension.FileExtensionId;
        //            }
        //        }
        //    }
        //    return success;
        //}

        //public override bool FuncPreEdit(Guid id, ref ProjectFileEditBindModel model, ref ProjectFile entity, ref JsonResponse<bool?> response)
        //{
        //    bool success = base.FuncPreEdit(id, ref model, ref entity, ref response);
        //    if (success)
        //    {
        //        if (model.FileExtensionName == null)
        //        {
        //            response = new JsonResponse<bool?>(Validation.InvalidModel, false);
        //            success = false;
        //        }
        //        else
        //        {
        //            var fileExtensionModel = new FileExtensionModel<FileExtension>();
        //            var fileExtension = fileExtensionModel.GetData(FileExtensionName: model.FileExtensionName, IsBlock: false, IsDeleted: false).SingleOrDefault();

        //            if (fileExtension == null)
        //            {
        //                var insertedExtension = fileExtensionModel.Insert(new FileExtension()
        //                {
        //                    FileExtensionId = Guid.NewGuid(),
        //                    FileExtensionName = model.FileExtensionName,
        //                    FileExtensionAltName = model.FileExtensionName,
        //                    CreateDate = DateTime.Now,
        //                    CreateUserId = Guid.Parse(User.Identity.GetUserId()),
        //                });

        //                entity.FileExtensionId = insertedExtension.FileExtensionId;
        //            }
        //            else
        //            {
        //                entity.FileExtensionId = fileExtension.FileExtensionId;
        //            }
        //        }
        //    }
        //    return success;
        //}

        [AllowAnonymous]
        public override IActionResult Details([FromRoute] Guid id, [FromRoute] Guid? notificationId)
        {
            return base.Details(id, notificationId);
        }

        #region Mapper
        public override PaginationResult<ProjectFileGridViewModel> FuncGetGridMapViewModel(PaginationResult<ProjectFileView> model)
        {
            return new PaginationResult<ProjectFileGridViewModel>()
            {
                TotalItemsCount = model.TotalItemsCount,
                PageItems = _mapper.Map<IEnumerable<ProjectFileGridViewModel>>(model.PageItems)
            };
        }
        public override ProjectFileDetailsViewModel FuncPreDetailsMapViewModel(ProjectFileView model)
        {
            return _mapper.Map<ProjectFileDetailsViewModel>(model);
        }
        public override ProjectFile FuncPreCreateMapModel(ProjectFileCreateBindModel model)
        {
            return _mapper.Map<ProjectFile>(model);
        }
        public override ProjectFile FuncPreEditMapModel(Guid id, ProjectFileEditBindModel model, ProjectFile entity)
        {
            entity = _mapper.Map(model, entity);
            return entity;
        }
        public override ProjectFileEditBindModel FuncPreInitEditViewMapEntity(ProjectFile entity)
        {
            return _mapper.Map<ProjectFileEditBindModel>(entity);
        }
        #endregion


        [AllowAnonymous]
        [HttpGet("Search/Filters")]
        public IActionResult GetSearchFilters()
        {
            var contentTypes = new ContentTypeModel<ContentType>()
                     .GetData(IsBlock: false, IsDeleted: false).OrderByDescending(x => x.CreateDate).Select(x =>
                     {
                         return new CustomSelectListItem()
                         {
                             Text = CoreUtility.GetDDLText(x.ContentTypeName, x.ContentTypeAltName),
                             AltText = CoreUtility.GetDDLAltText(x.ContentTypeName, x.ContentTypeAltName),
                             Value = x.ContentTypeId.ToString(),
                         };
                     });

            var categories = new CategoryModel<Category>()
                    .GetData(IsBlock: false, IsDeleted: false).Select(x =>
                    {
                        return new CustomSelectListItem()
                        {
                            Text = CoreUtility.GetDDLText(x.CategoryName, x.CategoryAltName),
                            AltText = CoreUtility.GetDDLAltText(x.CategoryName, x.CategoryAltName),
                            Value = x.CategoryId.ToString(),
                        };
                    });

            var projects = new ProjectModel<Project>()
                   .GetData(IsBlock: false, IsDeleted: false).Select(x =>
                   {
                       return new CustomSelectListItem()
                       {
                           Text = CoreUtility.GetDDLText(x.ProjectName, x.ProjectAltName),
                           AltText = CoreUtility.GetDDLAltText(x.ProjectName, x.ProjectAltName),
                           Value = x.ProjectId.ToString(),
                       };
                   });

            var fileExtensions = new FileExtensionModel<FileExtension>()
                   .GetData(IsBlock: false, IsDeleted: false).Select(x =>
                   {
                       return new CustomSelectListItem()
                       {
                           Text = CoreUtility.GetDDLText(x.FileExtensionName, x.FileExtensionAltName),
                           AltText = CoreUtility.GetDDLAltText(x.FileExtensionName, x.FileExtensionAltName),
                           Value = x.FileExtensionId.ToString(),
                       };
                   });

            var response = new ProjectFileGetSearchFilterRequestModel()
            {
                Categories = categories,
                ContentTypes = contentTypes,
                Projects = projects,
                FileExtensions = fileExtensions,
            };

            return Ok(new JsonResponse<ProjectFileGetSearchFilterRequestModel>(response));
        }

        [AllowAnonymous]
        [HttpPost("Search")]
        public IActionResult SearchProjectFiles([FromBody] ProjectFileFileSearchRequestModel model)
        {
            var projectFileModel = new ProjectFileModel<ProjectFileView>();

            var requestBody = new GenericDataFormat()
            {
                Filters = new List<GenericDataFormat.FilterItem>() 
                { 
                    new GenericDataFormat.FilterItem()
                    {
                        Property = "IsDeleted",
                        Value = false,
                        Operation= GenericDataFormat.FilterOperation.Equal,
                        LogicalOperation = GenericDataFormat.LogicalOperation.And,
                    },
                    new GenericDataFormat.FilterItem()
                    {
                        Property = "IsBlock",
                        Value = false,
                        Operation= GenericDataFormat.FilterOperation.Equal,
                        LogicalOperation = GenericDataFormat.LogicalOperation.And,
                    }
                },
                Paging = new GenericDataFormat.PagingItem()
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize,
                },
                Sorts = new List<GenericDataFormat.SortItems>()
                {
                    new GenericDataFormat.SortItems()
                    {
                        Priority = 1,
                        Property = "CreateDate",
                        SortType = GenericDataFormat.SortType.Desc,
                    }
                },
            };

            if (model.ProjectId != null)
            {
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "ProjectId", value: model.ProjectId));
            }

            if (model.FileExtensionIds != null && model.FileExtensionIds.Any())
            {
                requestBody.Filters.AddRange(CoreUtility.GetFilter(key: "FileExtensionId", values: model.FileExtensionIds.Select(x => (object)x), LogicalOperation: "Or"));
            }

            if (model.CategoryId != null)
            {
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "CategoryId", value: model.CategoryId));
            }

            if (model.ContentTypeId != null)
            {
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "ContentTypeId", value: model.ContentTypeId));
            }

            if (!string.IsNullOrEmpty(model.KeyWords))
            {
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "KeyWords", value: model.KeyWords, FilterOperation: "Like"));
            }

            if (!string.IsNullOrEmpty(model.Search))
            {
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "ProjectName", value: model.Search, FilterOperation: "Like", LogicalOperation: "Or"));
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "ContentTypeName", value: model.Search, FilterOperation: "Like", LogicalOperation: "Or"));
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "CategoryName", value: model.Search, FilterOperation: "Like", LogicalOperation: "Or"));
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "FileExtensionName", value: model.Search, FilterOperation: "Like", LogicalOperation: "Or"));
                requestBody.Filters.Add(CoreUtility.GetFilter(key: "ContentTitle", value: model.Search, FilterOperation: "Like", LogicalOperation: "Or"));
            }


            var result = projectFileModel.GetView_WithPaging(requestBody);

            var response = new PaginationResult<ProjectFileGridViewModel>()
            {
                TotalItemsCount = result.TotalItemsCount,
                PageItems = _mapper.Map<IEnumerable<ProjectFileGridViewModel>>(result.PageItems)
            };

            return Ok(new JsonResponse<PaginationResult<ProjectFileGridViewModel>>(response));
        }

    }
}
