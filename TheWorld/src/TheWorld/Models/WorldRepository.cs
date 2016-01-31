using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    using Microsoft.Extensions.Logging;

    public class WorldRepository : IWorldRepository
    {
        private WorldContext context;
        private ILogger<WorldRepository> logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return this.context.Trips.OrderBy(t => t.Name);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Could not get trips frin db", ex);
            }
            return new List<Trip>();
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return this.context.Trips
                .Include(t => t.Stops)
                .OrderBy(t => t.Name);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Could not get trips frin db", ex);
            }
            return new List<Trip>();
        }

        public void AddTrip(Trip trip)
        {
            this.context.Trips.Add(trip);
        }

        public bool SaveAll()
        {
            return this.context.SaveChanges() > 0;
        }

        public Trip GetTripByName(string tripName, string name)
        {
            return this.context.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(t => t.Name.ToLower() == tripName.ToLower() && t.UserName == name);
        }

        public void AddStop(string tripName, string user, Stop stop)
        {
            var trip = GetTripByName(tripName, user);
            trip.Stops.Add(stop);
            stop.Order = trip.Stops.Max(s => s.Order) + 1;
            this.context.Stops.Add(stop);
        }

        public IEnumerable<Trip> GetUserTripsWithStops(string name)
        {
            try
            {
                return this.context.Trips
                .Include(t => t.Stops)
                .OrderBy(t => t.Name)
                .Where(t => t.UserName == name).ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError("Could not get trips frin db", ex);
            }
            return new List<Trip>();
        }
    }
}
