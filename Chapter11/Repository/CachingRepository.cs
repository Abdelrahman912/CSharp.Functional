using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;

namespace Chapter11.Repository
{
    public class CachingRepository<T> : ICachingRepository<T>
    {
        private readonly IRepository<T> _db;
        private readonly IDictionary<int, T> _cache;
        public CachingRepository(IRepository<T> db , IDictionary<int,T> cache)
        {
            _db = db;
            _cache = cache;
        }

        public Option<T> Lookup(int id)
        {
           var result =  _cache.Lookup(id).OrElse(()=>_db.Lookup(id).Map(val =>
            {
                _cache.Add(id, val);
                return val;
            }));
            return result;
        }
        
    }
}
