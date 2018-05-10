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
    public class PointsRepository : IPointsRepository
    {
        private ApplicationDbContext context;

        public PointsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<List<Point>> GetPoints()
        {
            return context.Points.ToListAsync();
        }

        public Task<Point> GetPointByID(int id)
        {
            return context.Points.SingleOrDefaultAsync(r => r.Id == id);
        }

        public Task<Point> GetPointByName(string name)
        {
            return context.Points.SingleOrDefaultAsync(r => r.Name == name);
        }

        public void InsertPoint(Point point)
        {
            context.Points.Add(point);
        }

        public void DeletePoint(int pointID)
        {
            Point point = context.Points.Find(pointID);
            context.Points.Remove(point);
        }

        public void UpdatePoint(Point point)
        {
            context.Entry(point).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
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
