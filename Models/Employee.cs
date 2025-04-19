using System;

namespace Bigtoria.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname {  get; set; }
        public string Birthdate {  get; set; }
        public string Email {  get; set; }
        public string Phone { get; set; }
        public string Salary {  get; set; }
        public DateTime ContractDate {  get; set; }
        public bool Status {  get; set; }
        public string? ImagePath { get; set; }

        //Relaciones
        public LoginEmployee? LoginEmployee { get; set; }

        public int EmployeeTypeId {  get; set; }
        public EmployeeType EmployeeType { get; set; }

        public IEnumerable<Sale> Sales {  get; set; } 
    }
}
