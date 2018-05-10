using FarfetchExercise.Models;
using FarfetchExercise.Repository.Interfaces;
using FarfetchExercise.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests
{
    public class PathsServiceTests
    {
        [Fact]
        public async Task FindPathsBetweenNodes_ReturnsAListOfPaths()
        {
            // Arrange
            var mockPointsRepo = new Mock<IPointsRepository>();
            mockPointsRepo.Setup(repo => repo.GetPointByName("A")).Returns(Task.FromResult(GetTestPoints("A")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("B")).Returns(Task.FromResult(GetTestPoints("B")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("C")).Returns(Task.FromResult(GetTestPoints("C")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("D")).Returns(Task.FromResult(GetTestPoints("D")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("E")).Returns(Task.FromResult(GetTestPoints("E")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("F")).Returns(Task.FromResult(GetTestPoints("F")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("G")).Returns(Task.FromResult(GetTestPoints("G")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("H")).Returns(Task.FromResult(GetTestPoints("H")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("I")).Returns(Task.FromResult(GetTestPoints("I")));
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(1)).Returns(Task.FromResult(GetExerciseTestRoutes(1)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(2)).Returns(Task.FromResult(GetExerciseTestRoutes(2)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(3)).Returns(Task.FromResult(GetExerciseTestRoutes(3)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(4)).Returns(Task.FromResult(GetExerciseTestRoutes(4)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(5)).Returns(Task.FromResult(GetExerciseTestRoutes(5)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(6)).Returns(Task.FromResult(GetExerciseTestRoutes(6)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(7)).Returns(Task.FromResult(GetExerciseTestRoutes(7)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(8)).Returns(Task.FromResult(GetExerciseTestRoutes(8)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(9)).Returns(Task.FromResult(GetExerciseTestRoutes(9)));
            var service = new PathsService(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await service.FindPathsBetweenNodes("A", "B");

            // Assert
            var model = Assert.IsAssignableFrom<List<Path>>(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task FindPathsBetweenNodes_ReturnsAnList_WithoutDirectPaths()
        {
            // Arrange
            var mockPointsRepo = new Mock<IPointsRepository>();
            mockPointsRepo.Setup(repo => repo.GetPointByName("A")).Returns(Task.FromResult(GetTestPoints("A")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("B")).Returns(Task.FromResult(GetTestPoints("B")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("C")).Returns(Task.FromResult(GetTestPoints("C")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("D")).Returns(Task.FromResult(GetTestPoints("D")));
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(1)).Returns(Task.FromResult(GetTestRoutes(1)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(2)).Returns(Task.FromResult(GetTestRoutes(2)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(3)).Returns(Task.FromResult(GetTestRoutes(3)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(4)).Returns(Task.FromResult(GetTestRoutes(4)));
            var service = new PathsService(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await service.FindPathsBetweenNodes("A", "D");

            // Assert
            var model = Assert.IsAssignableFrom<List<Path>>(result);
            Assert.Equal(2, result.Count);
            foreach (Path p in result)
            {
                Assert.NotEqual(2, p.Points.Count);
            }
        }

        [Fact]
        public async Task FindPathsBetweenNodes_ReturnsAnEmptyList()
        {
            // Arrange
            var mockPointsRepo = new Mock<IPointsRepository>();
            mockPointsRepo.Setup(repo => repo.GetPointByName("A")).Returns(Task.FromResult(GetTestPoints("A")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("B")).Returns(Task.FromResult(GetTestPoints("B")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("C")).Returns(Task.FromResult(GetTestPoints("C")));
            mockPointsRepo.Setup(repo => repo.GetPointByName("D")).Returns(Task.FromResult(GetTestPoints("D")));
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(1)).Returns(Task.FromResult(GetTestRoutes(1)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(2)).Returns(Task.FromResult(GetTestRoutes(2)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(3)).Returns(Task.FromResult(GetTestRoutes(3)));
            mockRoutesRepo.Setup(repo => repo.GetRoutesByOrigin(4)).Returns(Task.FromResult(GetTestRoutes(4)));
            var service = new PathsService(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await service.FindPathsBetweenNodes("A", "A");

            // Assert
            var model = Assert.IsAssignableFrom<List<Path>>(result);
            Assert.Empty(result);
        }

        private Point GetTestPoints(string name)
        {
            switch (name)
            {
                case "A":
                    return new Point()
                    {
                        Id = 1,
                        Name = "A"
                    };
                case "B":
                    return new Point()
                    {
                        Id = 2,
                        Name = "B"
                    };
                case "C":
                    return new Point()
                    {
                        Id = 3,
                        Name = "C"
                    };
                case "D":
                    return new Point()
                    {
                        Id = 4,
                        Name = "D"
                    };
                case "E":
                    return new Point()
                    {
                        Id = 5,
                        Name = "E"
                    };
                case "F":
                    return new Point()
                    {
                        Id = 6,
                        Name = "F"
                    };
                case "G":
                    return new Point()
                    {
                        Id = 7,
                        Name = "G"
                    };
                case "H":
                    return new Point()
                    {
                        Id = 8,
                        Name = "H"
                    };
                case "I":
                    return new Point()
                    {
                        Id = 9,
                        Name = "I"
                    };
                default:
                    return null;
            }
        }

        private List<Route> GetExerciseTestRoutes(int originId)
        {
            var routes = new List<Route>();
            switch (originId)
            {
                case 1:
                    routes.AddRange(new Route[] {
                        new Route{ Name= "teste", Time= 10, Cost= 1, OriginId= 1, Origin= GetTestPoints("A"), DestinationId= 8, Destination= GetTestPoints("H") },
                        new Route{ Name= "teste", Time= 1, Cost= 20, OriginId= 1, Origin= GetTestPoints("A"), DestinationId= 3, Destination= GetTestPoints("C") },
                        new Route{ Name= "teste", Time= 30, Cost= 5, OriginId= 1, Origin= GetTestPoints("A"), DestinationId= 5, Destination= GetTestPoints("E") }
                    });
                    return routes;
                case 2:
                    return routes;
                case 3:
                    routes.AddRange(new Route[] {
                        new Route{ Name= "teste", Time= 1, Cost= 12, OriginId= 3, Origin= GetTestPoints("C"), DestinationId= 2, Destination= GetTestPoints("B") }
                    });
                    return routes;
                case 4:
                    routes.AddRange(new Route[] {
                        new Route { Name = "teste", Time = 4, Cost = 50, OriginId = 4, Origin= GetTestPoints("D"), DestinationId = 6, Destination= GetTestPoints("F") }
                    });
                    return routes;
                case 5:
                    routes.AddRange(new Route[] {
                        new Route { Name = "teste", Time = 3, Cost = 5, OriginId = 5, Origin= GetTestPoints("E"), DestinationId = 4, Destination= GetTestPoints("D") }
                    });
                    return routes;
                case 6:
                    routes.AddRange(new Route[] {
                        new Route { Name = "teste", Time = 45, Cost = 50, OriginId = 6, Origin= GetTestPoints("F"), DestinationId = 9, Destination= GetTestPoints("I") },
                        new Route { Name = "teste", Time = 40, Cost = 50, OriginId = 6, Origin= GetTestPoints("F"), DestinationId = 7, Destination= GetTestPoints("G") }
                    });
                    return routes;
                case 7:
                    routes.AddRange(new Route[] {
                        new Route { Name = "teste", Time = 64, Cost = 73, OriginId = 7, Origin= GetTestPoints("G"), DestinationId = 2, Destination= GetTestPoints("B") }
                    });
                    return routes;
                case 8:
                    routes.AddRange(new Route[] {
                        new Route { Name = "teste", Time = 30, Cost = 1, OriginId = 8, Origin= GetTestPoints("H"), DestinationId = 5, Destination= GetTestPoints("E") }
                    });
                    return routes;
                case 9:
                    routes.AddRange(new Route[] {
                        new Route { Name = "teste", Time = 65, Cost = 5, OriginId = 9, Origin= GetTestPoints("I"), DestinationId = 2, Destination= GetTestPoints("B") }
                    });
                    return routes;
                default:
                    return null;
            }
        }

        private List<Route> GetTestRoutes(int originId)
        {
            var routes = new List<Route>();
            switch (originId)
            {
                case 1:
                    routes.AddRange(new Route[] {
                        new Route{ Name= "teste", Time= 10, Cost= 1, OriginId= 1, Origin= GetTestPoints("A"), DestinationId= 2, Destination= GetTestPoints("B") },
                        new Route{ Name= "teste", Time= 1, Cost= 20, OriginId= 1, Origin= GetTestPoints("A"), DestinationId= 3, Destination= GetTestPoints("C") },
                        new Route{ Name= "teste", Time= 30, Cost= 5, OriginId= 1, Origin= GetTestPoints("A"), DestinationId= 4, Destination= GetTestPoints("D") }
                    });
                    return routes;
                case 2:
                    routes.AddRange(new Route[] {
                        new Route{ Name= "teste", Time= 30, Cost= 5, OriginId= 2, Origin= GetTestPoints("B"), DestinationId= 4, Destination= GetTestPoints("D") }
                    });
                    return routes;
                case 3:
                    routes.AddRange(new Route[] {
                        new Route{ Name= "teste", Time= 30, Cost= 5, OriginId= 3, Origin= GetTestPoints("C"), DestinationId= 4, Destination= GetTestPoints("D") }
                    });
                    return routes;
                case 4:
                    return routes;
                default:
                    return null;
            }
        }


        private List<Path> GetTestPaths()
        {
            List<Path> paths = new List<Path>();
            paths.Add(new Path()
            {
                Points = new List<string> { "A", "H", "E", "D", "F", "I", "B"},
                Time = 157,
                Cost = 112
            });
            paths.Add(new Path()
            {
                Points = new List<string> { "A", "H", "E", "D", "F", "G", "B"},
                Time = 151,
                Cost = 180
            });
            paths.Add(new Path()
            {
                Points = new List<string> { "A", "C", "B"},
                Time = 2,
                Cost = 32
            });
            paths.Add(new Path()
            {
                Points = new List<string> { "A", "E", "D", "F", "I", "B"},
                Time = 147,
                Cost = 115
            });
            paths.Add(new Path()
            {
                Points = new List<string> { "A", "E", "D", "F", "G", "B"},
                Time = 141,
                Cost = 183
            });
            return paths;
        }

    }
}

