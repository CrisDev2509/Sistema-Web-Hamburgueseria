namespace Bigtoria.Models
{
    public class LoginEmployee
    {
        public int Id { get; set; }
        public string Email {  get; set; }
        public string Password { get; set; }

        //Relaciones
        public int EmployeeId {  get; set; }
        public Employee Employee { get; set; }
    }
}
