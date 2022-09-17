using CSharp.Functional.Constructs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CSharp.Functional.Extensions.OptionExtension;
using static CSharp.Functional.Extensions.ValidationExtension;

namespace CSharp.Functional.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func) =>
           list.SelectMany(item => func(item).AsEnumerable());


        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list)
         => list.SelectMany(x => x);

        public static R Match<T, R>(this IEnumerable<T> list
         , Func<R> Empty, Func<T, IEnumerable<T>, R> Otherwise)
         => list.Head().Match(
            none: Empty,
            some: head => Otherwise(head, list.Skip(1)));

        public static Option<T> Head<T>(this IEnumerable<T> list)
        {
            if (list == null) return None;
            var enumerator = list.GetEnumerator();
            return enumerator.MoveNext() ? Some(enumerator.Current) : (Option<T>)None;
        }

        public static Option<IEnumerable<R>> Traverse<T, R>(this IEnumerable<T> ts, Func<T, Option<R>> f) =>
            ts.Aggregate(
                    seed: (Option<IEnumerable<R>>)Some(Enumerable.Empty<R>()),
                    func: (soFar, current) => from rs in soFar
                                              from r in f(current)
                                              select rs.Append(r));

        public static Validation<IEnumerable<R>> TraverseFailFast<T, R>(this IEnumerable<T> ts, Func<T, Validation<R>> f) =>
            ts.Aggregate(
                    seed: Valid(Enumerable.Empty<R>()),
                    func: (soFar, current) => from rs in soFar
                                              from r in f(current)
                                              select rs.Append(r));

        private static Func<IEnumerable<T>, T, IEnumerable<T>> Append<T>() =>
            (ts, t) => ts.Append(t);

        public static Validation<IEnumerable<R>> TraverseHarvest<T, R>(this IEnumerable<T> ts, Func<T, Validation<R>> f) =>
           ts.Aggregate(
                seed: Valid(Enumerable.Empty<R>()),
                func: (soFar, current) => Valid(Append<R>())
                                                .Apply(soFar)
                                                .Apply(f(current)));

        public static Task<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> ts, Func<T, Task<R>> f) =>
            ts.Aggregate(
                seed: Enumerable.Empty<R>().Async(),
                func: (soFar, current) => Append<R>().Async().Apply(soFar).Apply(f(current)));

        public static Task<IEnumerable<R>> Traverse<T, R>(this IEnumerable<T> ts, Func<Task, Task<R>> f) => Traverse(ts, f);

        public static Task<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> ts, Func<T, Task<R>> f) =>
            ts.Aggregate(
                    seed: Enumerable.Empty<R>().Async(),
                    func: (soFar, current) => from rs in soFar
                                              from r in f(current)
                                              select rs.Append(r));
    }
}
