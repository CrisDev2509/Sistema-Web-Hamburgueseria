using Bigtoria.Models;

namespace Bigtoria.ViewModels
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string CustomerName {  get; set; }
        public string EmpleyeeName {  get; set; }
        public DateTime Date {  get; set; }
        public string Status {  get; set; }
        public List<SaleDetail> Products { get; set; }
        public decimal Total {  get; set; }
        public bool IsSale {  get; set; }
    }
}
