using BOC.Core.Events;

namespace BOC.Core.Data.SqlEventStore
{
    public class SqlEventStore : IEventStore
    {
        private readonly BOCContext _context = new BOCContext();
        public IEnumerable<Event> GetEvents(Guid id)
        {
            var events = _context.Events.Where(e => e.EntityId == id).OrderBy(e => e.Timestamp.Date).ToList();
            return events;
        }

        public void Persist(Event e)
        {
            if (e != null)
            {
                _context.Events.Add(e);
                _context.SaveChanges();
            }
        }

        public void Persist(IEnumerable<Event> e)
        {
            if (e != null && e.Count() != 0)
            {
                _context.Events.AddRange(e);
                _context.SaveChanges();
            }
        }
    }
}
