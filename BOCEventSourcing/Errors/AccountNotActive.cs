using CSharp.Functional.Errors;

namespace BOC.Core.Errors
{
    public class AccountNotActive:Error
    {
        public override string Message => "Account is not active!!";
    }
}
