namespace Bigtoria.ViewModels
{
    public class HomeViewModel
    {
        public string Name { get; set; }
        public decimal TotalSales { get; set; } 
        public decimal Orders {  get; set; }
        public decimal Inventory {  get; set; }
        public int Delivery {  get; set; }
        public List<EmployeeRankViewModel> Employee { get; set; }
    }
}
