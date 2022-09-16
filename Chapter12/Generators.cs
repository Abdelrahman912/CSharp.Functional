using static CSharp.Functional.Functional;
using CSharp.Functional.Constructs;
using static CSharp.Functional.Extensions.OptionExtension;
using CSharp.Functional.Extensions;

namespace Chapter12
{
    public static class Generators
    {
        public static Generator<int> NextInt = (seed) =>
        {
            seed ^= seed >> 13;
            seed ^= seed << 18;
            int result = seed & 0xfffffff;
            return (result, result);
        };

        public static Generator<bool> NextBool =>
            NextInt.Map(i => i % 2 == 0);

        public static Generator<(int, int)> PairsOfInts =>
            from t in NextInt
            from r in NextInt
            select (t, r);

        public static Generator<Option<int>> OptionInt =>
            from some in NextBool
            from i in NextInt
           select some ? (Option<int>)Some(i) : None ;


    }
}
