using System;
using System.Runtime.InteropServices.ComTypes;

namespace CSharp.Functional.Extensions
{
    public static class FuncExtension
    {
        public static Func<R> Apply<T, R>(this Func<T, R> f, T t) => 
            () => f(t);

        public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> f, T1 t1) =>
            (t2) => f(t1, t2);

        public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> f, T1 t1) =>
            (t2, t3) => f(t1, t2, t3);


        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> f) =>
            t1 => t2 => f(t1, t2);

        public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> f) =>
           t1 => t2 => t3 => f(t1, t2, t3);

    }
}
