using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoreProject1.Models
{
    public class WorldRepository:IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting All Trips from the Database");
            return _context.Trips.ToList();
            
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips.Include(t=>t.Stops)
                   .FirstOrDefault(t => t.Name == tripName);
        }

        public Trip GetUserTripByName(string tripName, string name)
        {
            return _context.Trips.Include(t => t.Stops)
                  .FirstOrDefault(t => t.Name == tripName && t.UserName==name);
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;

        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetUserTripByName(tripName,username);
            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public IEnumerable<Trip> GetTripByUsername(string name)
        {
            return _context.Trips.Include(t=>t.Stops).Where(t => t.UserName == name).ToList();
        }

       
    }
}
