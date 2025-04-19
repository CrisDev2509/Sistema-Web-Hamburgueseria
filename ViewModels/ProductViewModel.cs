using Bigtoria.Models;

namespace Bigtoria.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowStore { get; set; }
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
        public decimal Discount { get; set; }
        public string? ImagePath { get; set; }
        public string CategoryName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
