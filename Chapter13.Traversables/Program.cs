using CSharp.Functional.Extensions;
namespace Chapter13.Traversables
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            //var nums = input.Split(',') //Array<string>
            //                .Select(x => x.Trim()) //IEnumerable<string>
            //                .Select(Double.Parse); //IEnumerable<Option<double>>

            //var sum = input.Split(',')
            //      .Select(String.Trim)
            //      .Traverse(Double.Parse)
            //      .Map(Enumerable.Sum)
            //      .Match(
            //         () => "Some of your inputs could not be parsed",
            //         (sum) => $"The sum is {sum}");

            //var sum = input.Split(',')
            //      .Select(String.Trim)
            //      .TraverseFailFast(Double.ParseV)
            //      .Map(Enumerable.Sum)
            //      .Match(
            //         (errs) => errs.First().Message,
            //         (sum) => $"The sum is {sum}");

            var sum = input.Split(',')
                  .Select(String.Trim)
                  .TraverseHarvest(Double.ParseV)
                  .Map(Enumerable.Sum)
                  .Match(
                     (errs) => errs.Aggregate("",(soFar,current)=>$"{soFar}{current.Message},"),
                     (sum) => $"The sum is {sum}");
        }
    }
}