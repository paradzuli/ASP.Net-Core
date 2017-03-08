using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreProject1.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();

        Trip GetTripByName(string tripName);

        void AddTrip(Trip trip);

        Task<bool> SaveChangesAsync();
        void AddStop(string tripName, Stop newStop,string username);
        IEnumerable<Trip> GetTripByUsername(string name);
        Trip GetUserTripByName(string tripName, string name);
    }
}
