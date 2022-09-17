using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chapter13
{
    public class Airline1 : IAirline
    {
        public Task<Flight> BestFare(string from, string to, DateTime on) =>
             Task.Run(() => new Flight(5000));

        public Task<IEnumerable<Flight>> Flights(string from, string to, DateTime on) =>
            Task.Run(() => Enumerable.Range(1, 10).Select(i => new Flight(i * 100)));
           
    }

    public class Airline2 : IAirline
    {
        public Task<Flight> BestFare(string from, string to, DateTime on) =>
             Task.Run(() => new Flight(4000));

        public Task<IEnumerable<Flight>> Flights(string from, string to, DateTime on) =>
            Task.Run(() => Enumerable.Range(1, 10).Select(i => new Flight(i * 50 + 50)));

    }
    public class Airline3 : IAirline
    {
        public Task<Flight> BestFare(string from, string to, DateTime on) =>
             Task.Run(() => new Flight(4500));

        public Task<IEnumerable<Flight>> Flights(string from, string to, DateTime on) =>
            Task.Run(() => Enumerable.Range(1, 10).Select(i => new Flight(i * 70 + 30)));

    }
}
