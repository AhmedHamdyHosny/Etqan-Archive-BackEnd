using AutoMapper;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using GenericRepositoryCore.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EtqanArchive.BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController<Category, Category, CategoryGridViewModel, CategoryDetailsViewModel,
        CategoryCreateModel, CategoryCreateBindModel, CategoryCreateBindModel, bool?, bool?, CategoryEditModel,
        CategoryEditBindModel, CategoryEditBindModel, bool?, Category,
        CategoryModel<Category>, CategoryModel<Category>>
    {
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override bool FuncPreGetGridView(ref GenericDataFormat options, ref JsonResponse<PaginationResult<CategoryGridViewModel>> response)
        {
            options.Includes = new GenericDataFormat.IncludeItems()
            {
                Properties = "CategoryId,CategoryName",
            };
            return base.FuncPreGetGridView(ref options, ref response);
        }

        #region Mapper

        public override PaginationResult<CategoryGridViewModel> FuncGetGridMapViewModel(PaginationResult<Category> model)
        {
            return new PaginationResult<CategoryGridViewModel>()
            {
                TotalItemsCount = model.TotalItemsCount,
                PageItems = _mapper.Map<IEnumerable<CategoryGridViewModel>>(model.PageItems)
            };
        }

        public override CategoryDetailsViewModel FuncPreDetailsMapViewModel(Category model)
        {
            return _mapper.Map<CategoryDetailsViewModel>(model);
        }


        public override Category FuncPreCreateMapModel(CategoryCreateBindModel model)
        {
            return _mapper.Map<Category>(model);
        }
        public override Category FuncPreEditMapModel(Guid id, CategoryEditBindModel model, Category entity)
        {
            entity = _mapper.Map(model, entity);
            return entity;
        }
        public override CategoryEditBindModel FuncPreInitEditViewMapEntity(Category entity)
        {
            return _mapper.Map<CategoryEditBindModel>(entity);
        }
        #endregion
    }
}
