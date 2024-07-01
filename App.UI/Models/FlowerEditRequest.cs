using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.UI.Models
{
    public class FlowerEditRequest
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Desc { get; set; }
        public double Price { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<int> RemovingPhotosIds { get; set; } = new List<int>();
        public List<IFormFile> NewPhotos { get; set; } = new List<IFormFile>();
        [JsonIgnore]
        public List<PhotoGetResponse>? PhotoUrls { get; set; }
    }
}
