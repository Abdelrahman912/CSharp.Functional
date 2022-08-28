using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = System.ValueTuple;
using System.Data.SqlClient;
using CSharp.Functional.Extensions;

namespace CSharp.Functional.Test
{
    public class ConnectionHelper
    {
        public static R Connect<R>
         (string connString, Func<SqlConnection, R> func)
        {
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                return func(conn);
            }
        }

        public static R Transact<R>
           (System.Data.SqlClient.SqlConnection conn, Func<SqlTransaction, R> f)
        {
            R r = default(R);
            using (var tran = conn.BeginTransaction())
            {
                r = f(tran);
                tran.Commit();
            }
            return r;
        }


        public static R Using<TDisp, R>(TDisp disposable
            , Func<TDisp, R> func) where TDisp : IDisposable
        {
            using (var disp = disposable) return func(disp);
        }

        public static Unit Using<TDisp>(TDisp disposable
           , Action<TDisp> act) where TDisp : IDisposable
           => Using(disposable, act.ToFunc());

        public static R Using<TDisp, R>(Func<TDisp> createDisposable
           , Func<TDisp, R> func) where TDisp : IDisposable
        {
            using (var disp = createDisposable()) return func(disp);
        }

        public static Unit Using<TDisp>(Func<TDisp> createDisposable
           , Action<TDisp> action) where TDisp : IDisposable
           => Using(createDisposable, action.ToFunc());

        public static class ConnectionHelper_V2
        {
            public static R Connect<R>(string connString, Func<IDbConnection, R> func)
               => Using(new SqlConnection(connString)
                  , conn => { conn.Open(); return func(conn); });
        }
    }
}
