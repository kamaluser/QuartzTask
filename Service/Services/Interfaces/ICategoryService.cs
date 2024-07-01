using Service.Dtos;
using Service.Dtos.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ICategoryService
    {
        int Create(CategoryCreateDto createDto);
        PaginatedList<CategoryGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10);
        void Edit(int id, CategoryEditDto editDto);
        void Delete(int id);
        CategoryGetDto GetById(int id);
        List<CategoryGetDto> GetAll(string? search = null);
    }
}
