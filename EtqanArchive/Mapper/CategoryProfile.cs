using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtqanArchive.BackEnd.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateBindModel, Category>();
            CreateMap<CategoryEditBindModel, Category>();
            CreateMap<Category, CategoryEditBindModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Category, CategoryDetailsViewModel>();
            CreateMap<Category, CategoryGridViewModel>();

        }
    }
}
