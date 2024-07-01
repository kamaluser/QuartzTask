using Service.Dtos;
using Service.Dtos.SliderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ISliderService
    {
        int Create(SliderCreateDto createDto);
        void Edit(int id, SliderEditDto editDto);
        void Delete(int id);
        SliderGetDto GetById(int id);
        PaginatedList<SliderGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10);
    }
}
