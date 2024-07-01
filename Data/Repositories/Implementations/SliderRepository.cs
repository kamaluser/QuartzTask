using Core.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class SliderRepository:Repository<Slider>, ISliderRepository     
    {
        public SliderRepository(AppDbContext context) : base(context)
        {

        }
    }
}
