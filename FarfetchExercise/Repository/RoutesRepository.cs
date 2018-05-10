using FarfetchExercise.Data;
using FarfetchExercise.Models;
using FarfetchExercise.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Repository
{
    public class RoutesRepository : IRoutesRepository
    {
        private ApplicationDbContext context;

        public RoutesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<List<Route>> GetRoutes()
        {
            return context.Routes.Include(r => r.Destination).Include(r => r.Origin).ToListAsync();
        }

        public Task<Route> GetRouteByID(int id)
        {
            return context.Routes
                .Include(r => r.Destination)
                .Include(r => r.Origin)
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        public Task<List<Route>> GetRoutesByOrigin(int id)
        {
            return context.Routes
                .Include(r => r.Destination)
                .Include(r => r.Origin)
                .Where(r => r.OriginId == id)
                .ToListAsync();
        }

        public Task<List<Route>> GetRoutesByDestination(int id)
        {
            return context.Routes
                .Include(r => r.Destination)
                .Include(r => r.Origin)
                .Where(r => r.DestinationId == id)
                .ToListAsync();
        }

        public void InsertRoute(Route route)
        {
            context.Routes.Add(route);
        }

        public void DeleteRoute(int routeID)
        {
            Route route = context.Routes.Find(routeID);
            context.Routes.Remove(route);
        }

        public void UpdateRoute(Route route)
        {
            context.Entry(route).State = EntityState.Modified;
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
