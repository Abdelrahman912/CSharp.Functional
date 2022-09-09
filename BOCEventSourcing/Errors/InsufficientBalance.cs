using CSharp.Functional.Errors;

namespace BOC.Core.Errors
{
    public class InsufficientBalance:Error
    {
        public override string Message =>
            "Sorry, Your account doesn't have sufficient balance";
    }
}
