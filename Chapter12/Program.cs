using System.Runtime.CompilerServices;
using static CSharp.Functional.Functional;
using Rates = System.Collections.Immutable.ImmutableDictionary<string, decimal>;
using CSharp.Functional.Extensions;

namespace Chapter12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainRec("Enter a currency pai like 'EURUSD', or 'q' to quit",Rates.Empty);
        }

        static void MainRec(string message , Rates cahce)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine()?.ToUpper();
            if (input == "Q" || input is null)
                return;
            GetRate( Yahoo.TryGetRate,input, cahce)
                .Run()
                .Match(
                Exception: ex=> MainRec($"Error: {ex.Message}" , cahce),
                Success: tuple => MainRec(tuple.Rate.ToString() , tuple.NewState)
                );
        }

        static Try<(decimal Rate,Rates NewState)> GetRate(Func<string,Try<decimal>> getRate , string ccyPair , Rates cache)
        {
            if (cache.ContainsKey(ccyPair))
                return () => (cache[ccyPair], cache);
            var tryRate = getRate(ccyPair);
            return  tryRate.Map(rate => (rate, cache.Add(ccyPair, rate)));
        }
    }
}