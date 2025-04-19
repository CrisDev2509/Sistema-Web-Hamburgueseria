namespace Bigtoria.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string Address {  get; set; }
        public string Reference {  get; set; }
        public int State {  get; set; }

        //Relaciones
        public int SaleId {  get; set; }
        public Sale Sale { get; set; }
    }
}
