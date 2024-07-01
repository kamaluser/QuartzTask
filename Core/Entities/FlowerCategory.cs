using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FlowerCategory
    {
        public int FlowerId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Flower Flower { get; set; }
    }
}
