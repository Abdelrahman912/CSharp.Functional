using CSharp.Functional.Constructs;

namespace Chapter11.Repository
{
    public interface IRepository<T>
    {
        Option<T> Lookup(int id);
    }
}
