using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;

namespace EtqanArchive.BackEnd.Mapper
{
    public class FileExtensionProfile : Profile
    {
        public FileExtensionProfile()
        {
            CreateMap<FileExtensionCreateBindModel, FileExtension>();
            CreateMap<FileExtensionEditBindModel, FileExtension>();
            CreateMap<FileExtension, FileExtensionEditBindModel>();
            //CreateMap<FileExtension, FileExtensionReferenceModel>();

        }
    }
}
