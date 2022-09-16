using CSharp.Functional.Constructs;
using System;
using static CSharp.Functional.Functional;

namespace CSharp.Functional.Extensions
{
    public static class GeneratorExtension
    {
        public static T Run<T>(this Generator<T> gen, int seed) =>
            gen(seed).Value;

        public static T Run<T>(this Generator<T> gen) =>
           gen(Environment.TickCount).Value;

        public static Generator<R> Map<T, R>(this Generator<T> gen, Func<T, R> f) => 
            (seed) =>
        {
            var (t, newSeed) = gen(seed);
            return (f(t), newSeed);
        };

        public static Generator<R> Select<T, R>(this Generator<T> gen, Func<T, R> f) => 
            gen.Map(f);

        public static Generator<R> Bind<T, R>(this Generator<T> gen, Func<T, Generator<R>> f) =>
            seed0 =>
            {
                var (t, seed1) = gen(seed0);
                return f(t)(seed1);
            };

        public static Generator<R> SelectMany<T, R>
         (this Generator<T> gen, Func<T, Generator<R>> f)
         => seed0 =>
         {
             var (t, seed1) = gen(seed0);
             return f(t)(seed1);
         };

        public static Generator<RR> SelectMany<T, R, RR>
            (this Generator<T> gen, Func<T, Generator<R>> bind, Func<T, R, RR> project) =>
            seed0 =>
            {
                var (t, seed1) = gen(seed0);
                var (r, seed2) = bind(t)(seed1);
                return (project(t,r), seed2);
            };
         


    }
}
