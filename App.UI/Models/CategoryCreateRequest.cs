using System.ComponentModel.DataAnnotations;

namespace App.UI.Models
{
    public class CategoryCreateRequest
    {
        [MaxLength(25)]
        public string  Name { get; set; }
    }
}
