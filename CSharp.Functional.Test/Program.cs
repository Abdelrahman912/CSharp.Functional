using System.Configuration;
using System.Security.Cryptography;
using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using static CSharp.Functional.Extensions.OptionExtension;


namespace CSharp.Functional.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ConnectionString connString = ConfigurationManager.ConnectionStrings["CompanyXYZ"].ConnectionString;
            // SqlTemplate sel = "SELECT * FROM EMPLOYEES",
            //      sqlById = $"{sel} WHERE ID = @Id",
            //      sqlByName = $"{sel} WHERE NAME = @Name";

            // var queryEmployees = connString.Query<Employee>();
            // var queryAllEmps = queryEmployees(sel, null).ToList();
            // var queryEmpsById = queryEmployees.Apply(sqlById);


            //Option<int> optX = Some(3);
            //Option<int> optY = Some(4);
            //Option<int> optZ = Some(5);
            //Func<int, int, int> multiply = (x, y) => x * y;
            //var result = ((Option<Func<int, int, int>>)Some(multiply))
            //                                           .Apply(optX)
            //                                           .Apply(optY)
            //                                           .ForEach(r=>Console.WriteLine(r));


            //optX.Map(multiply)
            //    .Apply(optY)
            //    .ForEach(r => Console.WriteLine(r));

            //var result = from x in optX
            //             from y in optY
            //             from z in optZ
            //             select x * y*z;

            //var result2 = optX.Bind(x => optY.Bind(y =>(Option<int>)Some( x * y)));

            var person = new Person(30, "Hamada");
            var newPerson = person.With(p => p.Age, 31);
        }
    }
}