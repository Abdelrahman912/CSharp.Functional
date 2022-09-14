using CSharp.Functional.Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharp.Functional.Functional;

namespace CSharp.Functional.Extensions
{
    public static class TryExtension
    {
        public static Exceptional<T> Run<T>(this Try<T> t)
        {
            try
            {
                var result = t();
                return result;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public static Try<R> Map<T, R>(this Try<T> t, Func<T, R> f) =>
            () => t.Run().Match<Exceptional<R>>(
                    ex => ex,
                    v => f(v));

        public static Try<R> Select<T, R>(this Try<T> @this, Func<T, R> func) => @this.Map(func);

        public static Try<R> Bind<T, R>(this Try<T> t, Func<T, Try<R>> f) =>
            ()=>  t.Run().Match(
                   ex => ex,
                   v => f(v).Run());

        public static Try<RR> SelectMany<T, R, RR>
        (this Try<T> @try, Func<T, Try<R>> bind, Func<T, R, RR> project)
        => ()
        => @try.Run().Match(
              ex => ex,
              t => bind(t).Run()
                       .Match<Exceptional<RR>>(
                          ex => ex,
                          r => project(t, r))
                       );
    }
}
