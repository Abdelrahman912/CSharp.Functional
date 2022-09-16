using static CSharp.Functional.Functional;

namespace Chapter12
{
    public static class Yahoo
    {
        private static decimal GetRate(string ccypair)
        {
            Console.WriteLine("fetching rate.....");
            var uri = $"http://finance.yahoo.com/d/quotes.csv?f=ll&s={ccypair}=X";
            var request = new HttpClient().GetStringAsync(uri);
            return 1.02m;//decimal.Parse(request.Result.Trim());
        }

        public static Try<decimal> TryGetRate(string ccypair) => 
            () => GetRate(ccypair);

    }
}
