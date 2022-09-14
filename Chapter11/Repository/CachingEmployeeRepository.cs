using Chapter11.Models;

namespace Chapter11.Repository
{
    public class CachingEmployeeRepository : CachingRepository<Employee>
    {
        public CachingEmployeeRepository()
            :base(new EmployeeRepository(),new Dictionary<int,Employee>())
        {

        }

    }
}
