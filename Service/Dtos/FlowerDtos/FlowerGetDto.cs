using Service.Dtos.CategoryDtos;
using Service.Dtos.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.FlowerDtos
{
    public class FlowerGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public List<PhotoGetDto> Photos { get; set; }
        public List<CategoryGetDto> Categories { get; set; }
    }
}
