using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using Dapper;
using static CSharp.Functional.Extensions.OptionExtension;
using static CSharp.Functional.Functional;
using Unit = System.ValueTuple;


namespace CSharp.Functional.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ConnectionString connString = ConfigurationManager.ConnectionStrings["CompanyXYZ"].ConnectionString;
            //SqlTemplate sel = "SELECT * FROM EMPLOYEES";

            //var emps1 = connString.Connect(c => c.Query<Employee>("SELECT * FROM EMPLOYEES")).ToList();

            //var logger = new ConsoleLogger();

            ////var emps2 = Trace(logger,
            ////            "Query Employees",
            ////            () => connString.Connect(c => c.Query<Employee>(sel)).ToList());

            //Middleware<int> logMw = cont => Trace(logger, "Query Employees", () => cont(1));
            //Middleware<SqlConnection> sqlMw = cont => connString.Connect(cont);
            //Func<SqlConnection, List<Employee>> empCont = c => c.Query<Employee>("SELECT * FROM EMPLOYEES").ToList();

            //var res = sqlMw.Map(empCont).Run();

            //var employees = logMw.Bind(_ => sqlMw).Map(empCont).Run();

            //SqlTemplate sqlById = $"{sel} WHERE ID = @Id",
            //     sqlByName = $"{sel} WHERE NAME = @Name";

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

            //var person = new Person(30, "Hamada");
            //var newPerson = person.With(p => p.Age, 31);
            FourMW();

        }

        public static void SingleMW()
        {
            ConnectionString connString = ConfigurationManager.ConnectionStrings["CompanyXYZ"].ConnectionString;
            SqlTemplate sel = "SELECT * FROM EMPLOYEES";
            Middleware<SqlConnection> sqlMw = cont => connString.Connect(cont);
            Func<SqlConnection, List<Employee>> empCont = c => c.Query<Employee>("SELECT * FROM EMPLOYEES").ToList();
            var employees = (from conn in sqlMw
                            select empCont(conn)).Run();
        }

        public static void DoubleMW()
        {
            ConnectionString connString = ConfigurationManager.ConnectionStrings["CompanyXYZ"].ConnectionString;
            SqlTemplate sel = "SELECT * FROM EMPLOYEES";
            var logger = new ConsoleLogger();

            Middleware<SqlConnection> sqlMw = cont => connString.Connect(cont);
            Func<SqlConnection, List<Employee>> empCont = c => c.Query<Employee>("SELECT * FROM EMPLOYEES").ToList();

            Func<string, Middleware<Unit>> Log = op => f=> Trace(logger,op,f.ToNullary());

            var employees = (from _ in Log("Query Employees")
                             from conn in sqlMw
                             select empCont(conn)).Run();
        }

        public static void ThreeMW()
        {
            ConnectionString connString = ConfigurationManager.ConnectionStrings["CompanyXYZ"].ConnectionString;
            SqlTemplate sel = "SELECT * FROM EMPLOYEES";
            var logger = new ConsoleLogger();

            Func<SqlConnection, List<Employee>> empCont = c => c.Query<Employee>("SELECT * FROM EMPLOYEES").ToList();
            Middleware<SqlConnection> sqlMw = cont => connString.Connect(cont);

            Func<string , Middleware<Unit>> Time = op => f => Program.Time(logger,op,f.ToNullary());

            Func<string, Middleware<Unit>> Trace = op => f => Program.Trace<dynamic>(logger, op, f.ToNullary());

            //var employees = (from u1 in Trace("Query Employees")
            //                 from u2 in Time("Query Employees")
            //                 from conn in sqlMw
            //                 select empCont(conn)).Run();

            var employees2 = Trace("Query Employees")
                                .Bind(_ => Time("Query Employees"))
                                .Bind(_ => sqlMw)
                                .Map(empCont)
                                .Run();


        }

        public static void FourMW()
        {
            ConnectionString connString = ConfigurationManager.ConnectionStrings["CompanyXYZ"].ConnectionString;
            SqlTemplate sel = "SELECT * FROM EMPLOYEES";
            var logger = new ConsoleLogger();

            Func<SqlConnection,SqlTransaction, List<Employee>> empCont = (c,t) => c.Query<Employee>("SELECT * FROM EMPLOYEES",transaction:t).ToList();
            Middleware<SqlConnection> sqlMw = cont =>
            {
                logger.LogTrace("Start Connection");
                var result = connString.Connect(cont);
                logger.LogTrace("End Connection");
                return result;
            };

            Func<string, Middleware<Unit>> Time = op => f => Program.Time(logger, op, f.ToNullary());

            Func<string, Middleware<Unit>> Trace = op => f => Program.Trace<dynamic>(logger, op, f.ToNullary());

            Func<SqlConnection, Middleware<SqlTransaction>> Transact = conn => f =>
            {
                logger.LogTrace("Starting Transaction");
                var result = ConnectionHelper.Transact(conn, f);
                logger.LogTrace("End Transaction");
                return result;
            };

            var employees = (from u1 in Trace("Query Employees")
                             from u2 in Time("Query Employees")
                             from conn in sqlMw
                             from trans in Transact(conn)
                             select empCont(conn,trans)).Run();
        }

        public static T Trace<T>(ILogger log, string op, Func<T> f)
        {
            log.LogTrace($"Entering {op}");
            var result = f();
            log.LogTrace($"Leaving {op}");
            return result;
        }

        public static T Time<T>(ILogger log, string op, Func<T> f)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = f();
            sw.Stop();
            log.LogTrace($"{op} took {sw.ElapsedMilliseconds} ms");
            return result;
        }

    }
}