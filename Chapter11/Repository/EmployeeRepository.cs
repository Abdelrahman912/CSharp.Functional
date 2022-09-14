using Chapter11.Models;
using CSharp.Functional.Constructs;
using static CSharp.Functional.Extensions.OptionExtension;

namespace Chapter11.Repository
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly List<Employee> _employees = new List<Employee>()
        {
            new Employee()
            {
                Id = 1,
                Name = "Ahmed",
                Salary = 4000,
            },
            new Employee()
            {
                Id = 2,
                Name ="Kamal",
                Salary = 5000
            },
            new Employee()
            {
                Id = 3,
                Name = "Abdo",
                Salary = 6000
            }
        };
        public Option<Employee> Lookup(int id)
        {
           var emp =  _employees.FirstOrDefault(emp => emp.Id == id);
            if (emp != null)
                return emp;
            return None;
        }
    }
}
