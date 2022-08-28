using System.Configuration;
using CSharp.Functional.Extensions;

namespace CSharp.Functional.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
           ConnectionString connString = ConfigurationManager.ConnectionStrings["CompanyXYZ"].ConnectionString;
            SqlTemplate sel = "SELECT * FROM EMPLOYEES",
                 sqlById = $"{sel} WHERE ID = @Id",
                 sqlByName = $"{sel} WHERE NAME = @Name";

            var queryEmployees = connString.Query<Employee>();
            var queryAllEmps = queryEmployees(sel, null).ToList();
            var queryEmpsById = queryEmployees.Apply(sqlById);
        }
    }
}