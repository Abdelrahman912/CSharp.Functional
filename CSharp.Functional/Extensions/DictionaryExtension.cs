using CSharp.Functional.Constructs;
using System.Collections.Generic;
using static CSharp.Functional.Extensions.OptionExtension;

namespace CSharp.Functional.Extensions
{
    public static class DictionaryExtension
    {
        public static Option<TValue>Lookup<Tkey,TValue>(this IDictionary<Tkey,TValue> dict,Tkey key)
        {
            TValue val = default(TValue);
            var result = dict.TryGetValue(key, out val);
            if (result)
                return val;
            return None;
        }
    }
}
