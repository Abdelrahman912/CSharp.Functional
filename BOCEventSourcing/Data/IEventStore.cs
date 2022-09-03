using BOC.Core.Events;

namespace BOC.Core.Data
{
    public interface IEventStore
    {
        void Persist(Event e);
        void Persist(IEnumerable<Event> e);
        IEnumerable<Event> GetEvents(Guid id);
    }
}
