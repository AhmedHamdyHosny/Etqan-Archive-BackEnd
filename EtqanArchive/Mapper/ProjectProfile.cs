using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;

namespace EtqanArchive.BackEnd.Mapper
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectCreateBindModel, Project>();
            CreateMap<ProjectEditBindModel, Project>();
            CreateMap<Project, ProjectEditBindModel>();
            CreateMap<Project, ProjectEditRequestModel>();
            CreateMap<ProjectEditRequestModel, Project>();
            CreateMap<Project, ProjectViewModel>();
            CreateMap<Project, ProjectDetailsViewModel>();
            CreateMap<Project, ProjectGridViewModel>();

        }
    }
}
