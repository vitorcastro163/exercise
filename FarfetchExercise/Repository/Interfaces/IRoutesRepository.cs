using FarfetchExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Repository.Interfaces
{
    public interface IRoutesRepository : IDisposable
    {
        Task<List<Route>> GetRoutes();
        Task<Route> GetRouteByID(int RouteId);
        Task<List<Route>> GetRoutesByOrigin(int RouteId);
        Task<List<Route>> GetRoutesByDestination(int RouteId);
        void InsertRoute(Route Route);
        void DeleteRoute(int RouteID);
        void UpdateRoute(Route Route);
        Task Save();
    }
}
