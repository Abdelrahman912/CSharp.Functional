using CSharp.Functional.Constructs;
using static CSharp.Functional.Extensions.OptionExtension;

namespace CSharp.Functional.Test
{
    public static class EmployeeExtension
    {
        private static bool AgeValidation(this Employee emp) =>
            emp.Age > 25 && emp.Age < 60;

        private static bool MarriedValidation(this Employee emp) =>
            emp.Status != MartialStatus.MARRIED;

        private static bool SalaryValidation(this Employee emp) =>
            emp.Salary >= 2500;


        public static bool LoanEligibility(Employee emp)
        {
            var employee = ((Option<Employee>)Some(emp)).Where(AgeValidation)
                                            .Where(MarriedValidation)
                                            .Where(SalaryValidation);
           return employee.Match(() => false,emp => true);
        }

    }
}
