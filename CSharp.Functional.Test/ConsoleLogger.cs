namespace CSharp.Functional.Test
{
    public class ConsoleLogger : ILogger
    {
        public void LogTrace(string s)
        {
            Console.WriteLine(s) ;
        }
    }
}
