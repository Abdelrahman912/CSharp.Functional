using System;
using static CSharp.Functional.Functional;

namespace CSharp.Functional.Extensions
{
    public static class MiddlewareExtension
    {
        public static Middleware<R> Bind<T, R>(this Middleware<T> mw, Func<T, Middleware<R>> f) =>
            cont => mw(t => f(t)(cont));

        public static Middleware<R> Map<T, R>(this Middleware<T> mw, Func<T, R> f) =>
            cont => mw(t => cont(f(t)));

        public static T Run<T>(this Middleware<T> mw) =>
            (T)mw(t => t);

        public static Middleware<R> Select<T, R>
        (this Middleware<T> mw, Func<T, R> f)
        => cont => mw(t => cont(f(t)));

        public static Middleware<R> SelectMany<T, R>
           (this Middleware<T> mw, Func<T, Middleware<R>> f)
           => cont => mw(t => f(t)(cont));

        public static Middleware<RR> SelectMany<T, R, RR>
           (this Middleware<T> @this, Func<T, Middleware<R>> f, Func<T, R, RR> project)
           => cont => @this(t => f(t)(r => cont(project(t, r))));
    }
}
