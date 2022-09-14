namespace Chapter11.Models
{
    public record Employee
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public decimal Salary { get; init; }
    }
}
