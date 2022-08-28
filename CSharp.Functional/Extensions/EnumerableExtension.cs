using CSharp.Functional.Constructs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp.Functional.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func) =>
           list.SelectMany(item => func(item).AsEnumerable());


        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list)
         => list.SelectMany(x => x);
    }
}
