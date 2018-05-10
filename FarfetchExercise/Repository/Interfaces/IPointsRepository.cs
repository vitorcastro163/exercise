using FarfetchExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Repository.Interfaces
{
    public interface IPointsRepository : IDisposable
    {
        Task<List<Point>> GetPoints();
        Task<Point> GetPointByID(int PointId);
        Task<Point> GetPointByName(String PointName);
        void InsertPoint(Point Point);
        void DeletePoint(int PointID);
        void UpdatePoint(Point Point);
        Task Save();
    }
}
