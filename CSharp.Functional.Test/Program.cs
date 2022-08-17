namespace CSharp.Functional.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var employee = new Employee()
            {
                Name = "Hamada",
                Age = 25 , 
                Status = MartialStatus.UNMARRIED,
                Salary = 3000
            };
           


            Console.WriteLine("Hello, World!");
        }
    }
}