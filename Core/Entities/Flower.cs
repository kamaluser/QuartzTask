using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Flower:AuditEntity
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public double DiscountPercent { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<FlowerCategory>? FlowerCategories { get; set; } = new List<FlowerCategory>();
    }
}
