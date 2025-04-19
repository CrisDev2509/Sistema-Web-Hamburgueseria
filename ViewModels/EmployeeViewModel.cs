
namespace Bigtoria.ViewModels
{
    public class EmployeeViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Salary { get; set; }
        public DateTime ContractDate { get; set; }
        public bool Status { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? Image { get; set; }
        public CategoryTypeViewModel CategoryType { get; set; }
    }
}
