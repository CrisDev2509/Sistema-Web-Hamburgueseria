using System.ComponentModel.DataAnnotations;

namespace Bigtoria.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string RUC {  get; set; }
        
        public string Email { get; set; }
    }
}
