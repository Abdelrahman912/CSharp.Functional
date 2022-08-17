namespace BOC.BLL
{
    public interface IValidator<T>
    {
        bool IsValid(T value);
    }
}
