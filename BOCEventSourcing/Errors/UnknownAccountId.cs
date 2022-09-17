using CSharp.Functional.Errors;

namespace BOC.Core.Errors
{
    public sealed class UnknownAccountId:Error
    {
        public Guid Id { get; }
        public UnknownAccountId(Guid id)
        {
            Id = id;
        }
        public override string Message => $"No account with id '{Id}' was found";
    }
}
