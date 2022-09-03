﻿using BOCEventSourcing.Events;

namespace BOCEventSourcing.Data
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly List<Event> _store = new List<Event>();
        public IEnumerable<Event> GetEvents(Guid id) =>
           _store.Where(e => e.Id.Equals(id)).OrderBy(e => e.Timestamp);

        public void Persist(Event e) =>
            _store.Add(e);


        public void Persist(IEnumerable<Event> e) =>
            _store.AddRange(e);

        public int Count() => _store.Count();

    }
}
