namespace CSharp.Functional.Test
{
    public record ConnectionString
    {
        public string Value { get; init; }
        public ConnectionString(string value)
        {
            Value = value;
        }

        public static implicit operator string(ConnectionString c) => c.Value;
        public static implicit operator ConnectionString(string value) => new ConnectionString(value);
        public override string ToString() =>
            Value;
       
    }
}
