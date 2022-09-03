using CSharp.Functional.Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using static CSharp.Functional.Extensions.OptionExtension;

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
    }
}
