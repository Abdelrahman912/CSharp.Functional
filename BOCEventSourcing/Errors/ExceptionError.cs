using CSharp.Functional.Errors;

namespace BOC.Core.Errors
{
    public sealed class ExceptionError:Error
    {
        public ExceptionError(Exception ex)
        {
            Exception = ex;
        }

        public Exception Exception { get; }
        public override string Message => Exception.Message;
    }
}
