namespace Bigtoria.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname {  get; set; }
        public string Email { get; set; }
        public string Phone {  get; set; }

        //Relaciones
        public IEnumerable<Sale> Sales { get; set; }
    }
}
