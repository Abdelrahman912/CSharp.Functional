using System;
using static CSharp.Functional.Functional;
using Unit = System.ValueTuple;

namespace CSharp.Functional.Extensions
{
    public static class FuncExtension
    {

        public static Func<T> ToNullary<T>(this Func<Unit, T> f) =>
            () => f(Unit());

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

        public static Func<R> Map<T, R>(this Func<T> f, Func<T, R> g) =>
            () => g(f());

        public static Func<R> Bind<T, R>(this Func<T> f, Func<T, Func<R>> g) =>
            g(f());

        public static Try<T> AsTry<T>(this Func<T> t) =>
            () => t();

    }
}
