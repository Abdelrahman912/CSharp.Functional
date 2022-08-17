using CSharp.Functional.Constructs;

namespace BOC.BLL
{
    public interface IRepository<T>
    {
        Option<T> Get(int id);
        void Save(int id, T t);
    }
}
