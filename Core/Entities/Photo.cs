using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Photo:BaseEntity
    {
        public string Name { get; set; }
        public int FlowerId { get; set; }
        public Flower Flower { get; set; }
    }
}
