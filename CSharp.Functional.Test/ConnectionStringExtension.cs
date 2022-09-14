using Dapper;
using System.Data.SqlClient;
using static CSharp.Functional.Test.ConnectionHelper;

namespace CSharp.Functional.Test
{
    public static class ConnectionStringExtension
    {

        public static R Connect<R>(this ConnectionString connString,Func<SqlConnection , R> f)
        {
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                return f(conn);
            }
        }

        public static Func<SqlTemplate, object, IEnumerable<T>> Query<T>(this ConnectionString connString) =>
            (sql, param) => Connect(connString, sqlConn => sqlConn.Query<T>(sql, param));

        public static void Execute(this ConnectionString connString
        , SqlTemplate sql, object param)
        => Connect(connString, conn => conn.Execute(sql, param));
    }
}
