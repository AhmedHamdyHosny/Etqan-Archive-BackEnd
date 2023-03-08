using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer;
using EtqanArchive.DataLayer.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtqanArchive.BackEnd.Services
{
    public interface IProjectService
    {
        bool PreEdit(Guid id, Guid userId, ProjectEditRequestModel model, ref Project entity, ref JsonResponse<bool?> response);
        Project Edit(Project entity);
    }
    public class ProjectService  : IProjectService
    {
        private readonly IMapper _mapper;

        public ProjectService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Project Edit(Project entity)
        {
            using (var context = new EtqanArchiveDBContext())
            {
                var oldEntity = context.Projects.Include(j => j.ProjectFiles)
                    .Single(j => j.ProjectId == entity.ProjectId && !j.IsBlock && !j.IsDeleted);
                if (entity.ProjectFiles != null)
                {
                    foreach (var childItem in entity.ProjectFiles)
                    {
                        var oldProjectFile = oldEntity.ProjectFiles.SingleOrDefault(c => c.ProjectFileId == childItem.ProjectFileId);
                        if (oldProjectFile != null)
                        {
                            context.Entry(oldProjectFile).CurrentValues.SetValues(childItem);
                        }
                        else
                        {
                            context.ProjectFiles.Add(childItem);
                        }
                    }
                    foreach (var oldChildItem in oldEntity.ProjectFiles)
                    {
                        if (!entity.ProjectFiles.Any(c => c.ProjectFileId == oldChildItem.ProjectFileId))
                            context.ProjectFiles.Remove(oldChildItem);
                    }
                }
                context.Entry(oldEntity).State = EntityState.Modified;
                context.SaveChanges();
            }
            return entity;
        }

        public bool PreEdit(Guid id, Guid userId, ProjectEditRequestModel model, ref Project entity, ref JsonResponse<bool?> response)
        {
            Project oldEntity = null;
            using (var context = new EtqanArchiveDBContext())
            {
                oldEntity = context.Projects.Include(j => j.ProjectFiles).SingleOrDefault(j => j.ProjectId == id && !j.IsBlock && !j.IsDeleted);
                if (oldEntity == null)
                {
                    response = new JsonResponse<bool?>(null, "Project not found", false);
                    return false;
                }
                if (model.ProjectFiles != null && model.ProjectFiles.Any())
                {
                    List<ProjectFile> projectFiles = new List<ProjectFile>();
                    foreach (var item in model.ProjectFiles.Where(x => x.ProjectFileId == Guid.Empty))
                    {
                        ProjectFile projectFile = _mapper.Map<ProjectFile>(item);
                        projectFile.ProjectFileId = Guid.NewGuid();
                        projectFile.Project = null;
                        projectFile.ProjectId = entity.ProjectId;
                        projectFile.CreateDate = DateTime.Now;
                        projectFile.CreateUserId = userId;
                        projectFiles.Add(projectFile);
                    }
                    foreach (var item in model.ProjectFiles.Where(x => oldEntity.ProjectFiles.Any(y => y.ProjectFileId == x.ProjectFileId)))
                    {
                        var fe = oldEntity.ProjectFiles.SingleOrDefault(x => x.ProjectFileId == item.ProjectFileId);
                        ProjectFile projectFile = CoreUtility.CopyEntity(fe);

                        var originProjectFile = context.ProjectFiles.SingleOrDefault(x => x.ProjectFileId == item.ProjectFileId);
                        projectFile = _mapper.Map(item, projectFile);
                        projectFile.Project = null;
                        if (Repository<ProjectFile>.IsChanged(
                            projectFile, originProjectFile, context, GenericRepositoryCoreConstant.UpdateReference_ExcludedProperties))
                        {
                            projectFile.ModifyDate = DateTime.Now;
                            projectFile.ModifyUserId = userId;
                        }
                        projectFiles.Add(projectFile);
                    }
                    entity.ProjectFiles = projectFiles;
                }
            }
            return true;
        }

    }
}
