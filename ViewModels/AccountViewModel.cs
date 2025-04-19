namespace Bigtoria.ViewModels
{
    public class AccountViewModel
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        //Información usuario
        public int userId {  get; set; }
        public string? Name {  get; set; }
        public string? Lastname {  get; set; }
        public string? Birthdate {  get; set; }
        public string? PersonalEmail {  get; set; }
        public string? PhoneNumber { get; set; }
        public string? Type {  get; set; }
    }
}
