namespace Bigtoria.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status {  get; set; }
        public bool ShowFilter {  get; set; }

        //Relaciones
        public IEnumerable<Product> Products { get; set; }
    }
}
