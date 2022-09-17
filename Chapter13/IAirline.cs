using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chapter13
{
    public record Flight(double Price);

    public interface IAirline
    {
        Task<IEnumerable<Flight>> Flights(string from, string to, DateTime on);
        Task<Flight> BestFare(string from, string to, DateTime on);
    }
}
