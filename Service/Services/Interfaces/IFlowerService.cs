using Service.Dtos.CategoryDtos;
using Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Dtos.FlowerDtos;

namespace Service.Services.Interfaces
{
    public interface IFlowerService
    {
        int Create(FlowerCreateDto createDto);
        PaginatedList<FlowerGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10);
        void Edit(int id, FlowerEditDto editDto);
        void Delete(int id);
        FlowerGetDto GetById(int id);
    }
}
