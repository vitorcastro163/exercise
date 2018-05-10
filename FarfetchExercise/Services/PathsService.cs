using FarfetchExercise.Data;
using FarfetchExercise.Models;
using FarfetchExercise.Repository;
using FarfetchExercise.Repository.Interfaces;
using FarfetchExercise.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Services
{
    public class PathsService : IPathsService
    {
        private IRoutesRepository _routesRepository;
        private IPointsRepository _pointsRepository;

        public PathsService(IRoutesRepository routesRepository, IPointsRepository pointsRepository)
        {
            _routesRepository = routesRepository;
            _pointsRepository = pointsRepository;
        }

        public async Task FindPathsBetweenNodes(string startNode, string endNode, List<string> visited, List<Point> points, int time, int cost, List<Path> paths)
        {
            visited.Add(startNode);

            if (startNode == endNode && points.Count > 1)
            {
                Path p = new Path(points, time, cost, endNode);
                paths.Add(p);
            }

            Point start = await _pointsRepository.GetPointByName(startNode);

            List<Route> routes = await _routesRepository.GetRoutesByOrigin(start.Id);

            foreach (Route route in routes)
            {
                if (!visited.Contains(route.Destination.Name))
                {
                    points.Add(route.Origin);

                    await FindPathsBetweenNodes(route.Destination.Name, endNode, visited, points, time + route.Time, cost + route.Cost, paths);

                    points.Remove(route.Origin);
                }

            }

            visited.Remove(startNode);
        }

        public async Task<List<Path>> FindPathsBetweenNodes(string startNode, string endNode)
        {
            List<Path> paths = new List<Path>();
            await FindPathsBetweenNodes(startNode, endNode, new List<string>(), new List<Point>(), 0, 0, paths);
            return paths;
        }
    }
}
