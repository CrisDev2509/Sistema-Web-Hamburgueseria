namespace Bigtoria.Models
{
    public class EmployeeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status {  get; set; }

        //Relaciones
        public IEnumerable<Employee> empleyoees { get; set; }
    }
}
