using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtqanArchive.BackEnd.Mapper
{
    public class FileExtensionProfile : Profile
    {
        public FileExtensionProfile()
        {
            CreateMap<FileExtensionCreateBindModel, FileExtension>();
            CreateMap<FileExtensionEditBindModel, FileExtension>();
            CreateMap<FileExtension, FileExtensionEditBindModel>();
            CreateMap<FileExtension, FileExtensionReferenceModel>();

        }
    }
}
