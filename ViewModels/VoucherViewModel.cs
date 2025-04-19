namespace Bigtoria.ViewModels
{
    public class VoucherViewModel
    {
        public string? Type { get; set; }
        public string? Name {  get; set; }
        public string? Lastname {  get; set; }
        public string? Email {  get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; } = "---";
        public string? Reference { get; set; } = "---";
        public string? Payment { get; set; }
    }
}
