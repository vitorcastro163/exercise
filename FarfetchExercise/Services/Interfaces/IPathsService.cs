using System.Collections.Generic;
using System.Threading.Tasks;
using FarfetchExercise.Models;

namespace FarfetchExercise.Services.Interfaces
{
    public interface IPathsService
    {
        Task<List<Path>> FindPathsBetweenNodes(string startNode, string endNode);
        Task FindPathsBetweenNodes(string startNode, string endNode, List<string> visited, List<Point> points, int time, int cost, List<Path> paths);
    }
}