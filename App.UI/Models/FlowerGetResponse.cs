namespace App.UI.Models
{
    public class FlowerGetResponse
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public List<PhotoGetResponse> Photos { get; set; }
        public List<CategoryGetResponse> Categories { get; set; }
    }
}
