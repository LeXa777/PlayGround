using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        void AddTrip(Trip trip);
        bool SaveAll();
        Trip GetTripByName(string tripName, string user);
        void AddStop(string tripName, string user, Stop stop);
        IEnumerable<Trip> GetUserTripsWithStops(string name);
    }
}
