using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.UI.Models
{
    public class SliderEditRequest
    {
        [MaxLength(100)]
        public string? Title { get; set; }
        [MaxLength(100)]
        public string? Desc { get; set; }
        public int? Order { get; set; }
        public IFormFile? ImageFile { get; set; }
        [JsonIgnore]
        public string? ImageUrl { get; set; }
    }
}
