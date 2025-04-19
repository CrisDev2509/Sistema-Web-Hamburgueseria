namespace Bigtoria.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public decimal Quantity {  get; set; }
        public decimal UnitPrice { get; set; }

        //Relaciones
        public int ProductId {  get; set; }
        public Product Product { get; set; }

        public int SaleId {  get; set; }
        public Sale Sale { get; set; }
    }
}
