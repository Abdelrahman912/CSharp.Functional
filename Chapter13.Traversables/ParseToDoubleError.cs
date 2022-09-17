using CSharp.Functional.Errors;

namespace Chapter13.Traversables
{
    public class ParseToDoubleError:Error   
    {
        public ParseToDoubleError(string s)
        {
            Message = $"Failed to parse '{s}' to double";
        }
        public override string Message { get; }
    }
}
