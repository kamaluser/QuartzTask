using System.ComponentModel.DataAnnotations;

namespace App.UI.Models
{
    public class FlowerCreateRequest
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Desc { get; set; }
        public double Price { get; set; }
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
