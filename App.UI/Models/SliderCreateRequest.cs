using System.ComponentModel.DataAnnotations;

namespace App.UI.Models
{
    public class SliderCreateRequest
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Desc { get; set; }
        public int Order { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
