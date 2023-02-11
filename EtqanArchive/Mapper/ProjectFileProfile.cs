using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;
using EtqanArchive.DataLayer.ViewEntity;

namespace EtqanArchive.BackEnd.Mapper
{
    public class ProjectFileProfile : Profile
    {
        public ProjectFileProfile()
        {
            CreateMap<ProjectFileCreateBindModel, ProjectFile>();
            CreateMap<ProjectFileEditBindModel, ProjectFile>();
            CreateMap<ProjectFile, ProjectFileEditBindModel>();
            CreateMap<ProjectFileEditRequestModel, ProjectFile>();
            CreateMap<ProjectFile, ProjectFileEditRequestModel>();
            CreateMap<ProjectFileView, ProjectFileViewModel>();
            CreateMap<ProjectFileView, ProjectFileDetailsViewModel>();
            CreateMap<ProjectFileView, ProjectFileGridViewModel>();
        }

    }
}
