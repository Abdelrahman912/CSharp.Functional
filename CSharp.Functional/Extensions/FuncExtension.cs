using System;
using System.Runtime.InteropServices.ComTypes;

namespace CSharp.Functional.Extensions
{
    public static class FuncExtension
    {
        public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> f, T1 t1) =>
            (t2) => f(t1, t2);

        public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> f, T1 t1) =>
            (t2, t3) => f(t1, t2, t3);

    }
}
