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
    public class ProjectController : BaseApiController<Project, Project, ProjectGridViewModel, ProjectDetailsViewModel,
        ProjectCreateModel, ProjectCreateBindModel, ProjectCreateBindModel, bool?, bool?, ProjectEditModel,
        ProjectEditBindModel, ProjectEditRequestModel, bool?, Project,
        ProjectModel<Project>, ProjectModel<Project>>
    {
        private readonly IMapper _mapper;

        public ProjectController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override bool FuncPreGetGridView(ref GenericDataFormat options, ref JsonResponse<PaginationResult<ProjectGridViewModel>> response)
        {
            options.Includes = new GenericDataFormat.IncludeItems()
            {
                Properties = "ProjectId,ProjectName,ProjectLocation",
            };
            return base.FuncPreGetGridView(ref options, ref response);
        }

        public override bool FuncPreCreate(ref ProjectCreateBindModel model, ref Project entity, ref JsonResponse<bool?> response)
        {
            bool success = base.FuncPreCreate(ref model, ref entity, ref response);
            if (success)
            {
                entity.ProjectFiles = model.ProjectFiles.Select(x =>
                {
                    var projectFile = _mapper.Map<ProjectFile>(x);
                    projectFile.CreateDate = DateTime.Now;
                    projectFile.CreateUserId = Guid.Parse(User.Identity.GetUserId());
                    return projectFile;
                }).ToList();
            }
            return success;
        }

        public override IActionResult FuncPostInitEditView(Guid id, ref ProjectEditModel model, Guid? notificationId, Guid? taskId, ref JsonResponse<ProjectEditModel> response)
        {
            var projectFiles = new ProjectFileModel<ProjectFile>().GetData(ProjectId: id, IsBlock: false, IsDeleted: false, 
                IncludeReferences: "Category,FileExtension,FileExtension.ContentType");
            model.Item.ProjectFiles = _mapper.Map<IEnumerable<ProjectFileEditBindModel>>(projectFiles);
            return base.FuncPostInitEditView(id, ref model, notificationId, taskId, ref response);
        }

        public override bool FuncPreEdit(Guid id, ref ProjectEditRequestModel model, ref Project entity, ref JsonResponse<bool?> response)
        {
            IsEdit_WithReference = true;
            EntityReferences = "ProjectFiles";

            bool success = base.FuncPreEdit(id, ref model, ref entity, ref response);
            if (success)
            {
                Guid userId = Guid.Parse(User.Identity.GetUserId());
                var projectModel = new ProjectModel<Project>();
                Project oldEntity = projectModel.GetData(ProjectId: id, IncludeReferences: EntityReferences).SingleOrDefault();
                if (model.ProjectFiles != null && model.ProjectFiles.Any())
                {
                    List<ProjectFile> projectFiles = new List<ProjectFile>();
                    //set create user, create date for new items
                    foreach (var item in model.ProjectFiles.Where(x => x.ProjectFileId == Guid.Empty))
                    {
                        ProjectFile projectFile = _mapper.Map<ProjectFile>(item);
                        //projectFile.ProjectFileId = Guid.NewGuid();
                        projectFile.ProjectId = id;
                        projectFile.CreateDate = DateTime.Now;
                        projectFile.CreateUserId = userId;
                        projectFiles.Add(projectFile);
                    }

                    //loop over items that exist in old model items (search by id)
                    foreach (var item in model.ProjectFiles.Where(x => oldEntity.ProjectFiles.Any(y =>
                    y.ProjectFileId.ToString() == x.ProjectFileId.ToString())))
                    {
                        //get old item
                        var projectFile = oldEntity.ProjectFiles.SingleOrDefault(x =>
                        x.ProjectFileId.ToString() == item.ProjectFileId.ToString());
                        var originFileExtension = oldEntity.ProjectFiles.SingleOrDefault(x =>
                        x.ProjectFileId.ToString() == item.ProjectFileId.ToString());
                        //map updated properties values
                        projectFile = _mapper.Map(item, projectFile);
                        projectFile.Project = null;
                        //eventTicketClass = _mapper.Map<EventTicketClass>(item);
                        //check item if modified from latest time
                        if (Repository<ProjectFile>.IsChanged(
                            projectFile, originFileExtension, projectModel.dbContext, GenericRepositoryCoreConstant.UpdateReference_ExcludedProperties))
                        {
                            //set modify user, modify date for updated items
                            projectFile.ModifyDate = DateTime.Now;
                            projectFile.ModifyUserId = userId;
                        }
                        projectFiles.Add(projectFile);
                    }
                    entity.ProjectFiles = projectFiles;
                }
            }
            return success;
        }

        public override IActionResult FuncPostDetailsView(bool success, Guid id, ref ProjectDetailsViewModel model, Guid? notificationId, ref JsonResponse<ProjectDetailsViewModel> response)
        {
            if (success)
            {
                var projectFiles = new ProjectFileModel<ProjectFile>().GetData(ProjectId: id, IsBlock: false, IsDeleted: false,
                    IncludeReferences: "Category,FileExtension,FileExtension.ContentType");
                model.ProjectFiles = _mapper.Map<IEnumerable<ProjectFileEditBindModel>>(projectFiles);
            }
            return base.FuncPostDetailsView(success, id, ref model, notificationId, ref response);
        }

        #region Mapper

        public override PaginationResult<ProjectGridViewModel> FuncGetGridMapViewModel(PaginationResult<Project> model)
        {
            return new PaginationResult<ProjectGridViewModel>()
            {
                TotalItemsCount = model.TotalItemsCount,
                PageItems = _mapper.Map<IEnumerable<ProjectGridViewModel>>(model.PageItems)
            };
        }

        public override ProjectDetailsViewModel FuncPreDetailsMapViewModel(Project model)
        {
            return _mapper.Map<ProjectDetailsViewModel>(model);
        }


        public override Project FuncPreCreateMapModel(ProjectCreateBindModel model)
        {
            return _mapper.Map<Project>(model);
        }

        public override Project FuncPreEditMapModel(Guid id, ProjectEditRequestModel model, Project entity)
        {
            entity = _mapper.Map(model, entity);
            return entity;
        }
        public override ProjectEditBindModel FuncPreInitEditViewMapEntity(Project entity)
        {
            return _mapper.Map<ProjectEditBindModel>(entity);
        }

        #endregion

        public override bool FuncPreDeleteForever(IEnumerable<Guid> id_lst, ref JsonResponse<bool> response)
        {
            return base.FuncPreDeleteForever(id_lst, ref response);
        }

    }
}
