using Bigtoria.Models;

namespace Bigtoria.ViewModels
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }
        public string ClientName {  get; set; }
        public string Detail {  get; set; }
        public DateTime Date {  get; set; }
        public int SaleId {  get; set; }
        public int Status { get; set; }
        public string Address {  get; set; }
        public string Payment {  get; set; }
        public string Reference { get; set; }
        public List<DetailViewModel> Products { get; set; }
    }
}
