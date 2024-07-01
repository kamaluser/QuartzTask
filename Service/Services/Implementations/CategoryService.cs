using AutoMapper;
using Core.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Dtos;
using Service.Dtos.CategoryDtos;
using Service.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Services.Implementations
{
    public class CategoryService:ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public int Create(CategoryCreateDto dto)
        {
            if (_repository.Exists(x => x.Name.ToLower() == dto.Name.ToLower()))
                throw new RestException(StatusCodes.Status400BadRequest, "Name", "Category already exists.");

            Category category = _mapper.Map<Category>(dto);

            _repository.Add(category);
            _repository.Save();

            return category.Id;
        }

        public List<CategoryGetDto> GetAll(string? search = null)
        {
            var categories = _repository.GetAll(x => search == null || x.Name.Contains(search)).ToList();
            return _mapper.Map<List<CategoryGetDto>>(categories);
        }

        public PaginatedList<CategoryGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10)
        {
            var query = _repository.GetAll(x => search == null || x.Name.Contains(search));
            var paginated = PaginatedList<Category>.Create(query, page, size);
            if (page > paginated.TotalPages)
            {
                page = paginated.TotalPages;
                paginated = PaginatedList<Category>.Create(query, page, size);
            }
            return new PaginatedList<CategoryGetDto>(_mapper.Map<List<CategoryGetDto>>(paginated.Items), paginated.TotalPages, page, size);
        }

        public CategoryGetDto GetById(int id)
        {
            Category entity = _repository.Get(x => x.Id == id);

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Category not found");

            return _mapper.Map<CategoryGetDto>(entity);
        }

        public void Delete(int id)
        {
            Category entity = _repository.Get(x => x.Id == id);

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Category not found");

            _repository.Delete(entity);
            _repository.Save();
        }

        public void Edit(int id, CategoryEditDto updateDto)
        {
            Category entity = _repository.Get(x => x.Id == id);

            if (entity == null)
                throw new RestException(StatusCodes.Status404NotFound, "Category not found");


            if (entity.Name != updateDto.Name && _repository.Exists(x => x.Name == updateDto.Name))
                throw new RestException(StatusCodes.Status400BadRequest, "Name", "Category by this name already exists.");

            entity.Name = updateDto.Name;
            _repository.Save();
        }
    }

   

}
