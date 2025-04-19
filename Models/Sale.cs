namespace Bigtoria.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public byte SaleType {  get; set; }
        public string Payment { get; set; }
        public decimal PercentageIGV { get; set; }
        public decimal SubTotal { get; set; }
        public decimal IGV {  get; set; }
        public decimal Total { get; set; }
        public int State { get; set; }

        //Relaciones
        public SaleDetail SaleDetail { get; set; }
        public Delivery Delivery { get; set; }

        public int EmployeeId {  get; set; }
        public Employee Employee { get; set; }

        public int? ClientId {  get; set; }
        public Client Client { get; set; }
    }
}
