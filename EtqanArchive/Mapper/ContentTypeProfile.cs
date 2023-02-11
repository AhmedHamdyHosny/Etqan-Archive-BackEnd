using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;

namespace EtqanArchive.BackEnd.Mapper
{
    public class ContentTypeProfile : Profile
    {
        public ContentTypeProfile()
        {
            CreateMap<ContentTypeCreateBindModel, ContentType>();
            CreateMap<ContentTypeEditBindModel, ContentType>();
            CreateMap<ContentType, ContentTypeEditBindModel>();
            CreateMap<ContentType, ContentTypeViewModel>();
            CreateMap<ContentType, ContentTypeDetailsViewModel>();
            CreateMap<ContentType, ContentTypeGridViewModel>();
            CreateMap<ContentType, ContentTypeReferenceModel>();

        }
    }
}
