namespace CSharp.Functional.Test
{
    public enum MartialStatus
    {
        MARRIED,UNMARRIED
    }

    public record Employee
    {
        public string Name { get; init; }
        public int Age { get; init; }
        public decimal Salary { get; init; }
        public MartialStatus Status { get; init; }
    }
}
