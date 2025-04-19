namespace Bigtoria.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public decimal Ammout { get; set; }
        public byte Type { get; set; }
        public DateTime Date { get; set; }
        public int State { get; set; }

        public List<EmployeeReportViewModel> Employees { get; set; }
    }
}
