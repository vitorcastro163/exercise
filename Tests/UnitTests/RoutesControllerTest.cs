using FarfetchExercise.Controllers;
using FarfetchExercise.Models;
using FarfetchExercise.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Tests.UnitTests
{
    public class RoutesControllerTest
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfRoutes()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRoutes()).Returns(Task.FromResult(GetTestRoutes()));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Route>>(viewResult.ViewData.Model);
            Assert.Equal(GetTestRoutes().Count, model.Count());
        }

        [Fact]
        public async Task DetailsGet_ReturnsAViewResult_WithRoute()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(1)).Returns(Task.FromResult(GetTestRoutes()[0]));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Route>(viewResult.ViewData.Model);
            Assert.Equal(GetTestRoutes()[0], model);
        }

        [Fact]
        public async Task DetailsGet_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DetailsGet_ReturnsNotFound_WhenRouteReturnedIsNull()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(1)).Returns(Task.FromResult((Route)null));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateGet_ReturnsAnEmptyViewResult()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            var mockPointsRepo = new Mock<IPointsRepository>();
            mockPointsRepo.Setup(repo => repo.GetPoints()).Returns(Task.FromResult(GetTestPoints()));
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(2, viewResult.ViewData.Count);
        }

        [Fact]
        public async Task CreatePost_ReturnsAViewResult_WithRoute_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            var mockPointsRepo = new Mock<IPointsRepository>();
            mockPointsRepo.Setup(repo => repo.GetPoints()).Returns(Task.FromResult(GetTestPoints()));
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);
            controller.ModelState.AddModelError("Name", "Required");
            var newRoute = new Route() { OriginId = 1, DestinationId = 1 };

            // Act
            var result = await controller.Create(newRoute);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(2, viewResult.ViewData.Count);
            var model = Assert.IsAssignableFrom<Route>(
                viewResult.ViewData.Model);
            Assert.Equal(newRoute, model);
        }

        [Fact]
        public async Task CreatePost_ReturnsARedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.InsertRoute(It.IsAny<Route>())).Verifiable();
            mockRoutesRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);
            var newRoute = GetTestRoutes()[0];

            // Act
            var result = await controller.Create(newRoute);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRoutesRepo.Verify();
        }

        [Fact]
        public async Task EditGet_ReturnsAViewResult_WithRoute()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(1)).Returns(Task.FromResult(GetTestRoutes()[0]));
            var mockPointsRepo = new Mock<IPointsRepository>();
            mockPointsRepo.Setup(repo => repo.GetPoints()).Returns(Task.FromResult(GetTestPoints()));
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(2, viewResult.ViewData.Count);
            var model = Assert.IsAssignableFrom<Route>(viewResult.ViewData.Model);
            Assert.Equal(GetTestRoutes()[0], model);
        }

        [Fact]
        public async Task EditGet_ReturnsANotFoundResult_WhenIdIsNull()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsANotFoundResult_WhenRouteReturnedIsNull()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(1)).Returns(Task.FromResult((Route)null));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsANotFoundResult_WhenIdIsDifferentFromRouteId()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Edit(2, GetTestRoutes()[0]);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsAViewResult_WithRoute_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            var mockPointsRepo = new Mock<IPointsRepository>();
            mockPointsRepo.Setup(repo => repo.GetPoints()).Returns(Task.FromResult(GetTestPoints()));
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);
            controller.ModelState.AddModelError("Name", "Required");
            var newRoute = new Route() { Id = 1 };

            // Act
            var result = await controller.Edit(newRoute.Id, newRoute);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData);
            Assert.Equal(2, viewResult.ViewData.Count);
            var model = Assert.IsAssignableFrom<Route>(viewResult.ViewData.Model);
            Assert.Equal(newRoute, model);
        }

        [Fact]
        public async Task EditPost_ReturnsARedirectToActionResult_WhenModelStateIsValid_AndNoErrorsHappenSaving()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.UpdateRoute(It.IsAny<Route>())).Verifiable();
            mockRoutesRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);
            var newRoute = GetTestRoutes()[0];

            // Act
            var result = await controller.Edit(newRoute.Id, newRoute);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRoutesRepo.Verify();
        }

        [Fact]
        public async Task EditPost_ReturnsANotFoundResult_WhenModelStateIsValid_AndErrorsHappenSaving_AndRouteDoesNotExist()
        {
            // Arrange
            List<IUpdateEntry> entries = new List<IUpdateEntry>
            {
                null
            };
            string message = "Error message";
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.UpdateRoute(It.IsAny<Route>())).Throws(new DbUpdateConcurrencyException(message, entries));
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(It.IsAny<int>())).Returns(Task.FromResult((Route)null));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);
            var newRoute = GetTestRoutes()[0];

            // Act
            var result = await controller.Edit(newRoute.Id, newRoute);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ThrowsError_WhenModelStateIsValid_AndErrorsHappenSaving_AndRouteExists()
        {
            // Arrange
            List<IUpdateEntry> entries = new List<IUpdateEntry>
            {
                null
            };
            string message = "Error message";
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.UpdateRoute(It.IsAny<Route>())).Throws(new DbUpdateConcurrencyException(message, entries));
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(It.IsAny<int>())).Returns(Task.FromResult(GetTestRoutes()[0]));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);
            var newRoute = GetTestRoutes()[0];

            // Act
            var ex = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => controller.Edit(newRoute.Id, newRoute));

            // Assert
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public async Task DeleteGet_ReturnsAViewResult_WithRoute()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(1)).Returns(Task.FromResult(GetTestRoutes()[0]));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Route>(viewResult.ViewData.Model);
            Assert.Equal(GetTestRoutes()[0], model);
        }

        [Fact]
        public async Task DeleteGet_ReturnsANotFoundResult_WhenIdIsNull()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGet_ReturnsANotFoundResult_WhenRouteReturnedIsNull()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.GetRouteByID(1)).Returns(Task.FromResult((Route)null));
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task DeletePost_ReturnsARedirectToActionResult()
        {
            // Arrange
            var mockRoutesRepo = new Mock<IRoutesRepository>();
            mockRoutesRepo.Setup(repo => repo.DeleteRoute(It.IsAny<int>())).Verifiable();
            mockRoutesRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            var mockPointsRepo = new Mock<IPointsRepository>();
            var controller = new RoutesController(mockRoutesRepo.Object, mockPointsRepo.Object);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRoutesRepo.Verify();
        }

        private List<Point> GetTestPoints()
        {
            var points = new List<Point>();
            points.Add(new Point()
            {
                Id = 1,
                Name = "A"
            });
            points.Add(new Point()
            {
                Id = 2,
                Name = "B"
            });
            points.Add(new Point()
            {
                Id = 3,
                Name = "C"
            });
            points.Add(new Point()
            {
                Id = 4,
                Name = "D"
            });
            points.Add(new Point()
            {
                Id = 5,
                Name = "A"
            });
            points.Add(new Point()
            {
                Id = 6,
                Name = "E"
            });
            points.Add(new Point()
            {
                Id = 7,
                Name = "F"
            });
            points.Add(new Point()
            {
                Id = 8,
                Name = "G"
            });
            points.Add(new Point()
            {
                Id = 9,
                Name = "H"
            });
            points.Add(new Point()
            {
                Id = 10,
                Name = "I"
            });
            return points;
        }
        private List<Route> GetTestRoutes()
        {
            var routes = new List<Route>();
            routes.AddRange(new Route[] {
                new Route{ Name= "teste", Time= 10, Cost= 1, OriginId= 1, Origin= GetTestPoints()[0], DestinationId= 8, Destination= GetTestPoints()[7] },
                new Route{ Name= "teste", Time= 1, Cost= 20, OriginId= 1, Origin= GetTestPoints()[0], DestinationId= 3, Destination= GetTestPoints()[2] },
                new Route{ Name= "teste", Time= 30, Cost= 5, OriginId= 1, Origin= GetTestPoints()[0], DestinationId= 5, Destination= GetTestPoints()[4] },
                new Route{ Name= "teste", Time= 1, Cost= 12, OriginId= 3, Origin= GetTestPoints()[2], DestinationId= 2, Destination= GetTestPoints()[1] },
                new Route { Name = "teste", Time = 4, Cost = 50, OriginId = 4, Origin= GetTestPoints()[3], DestinationId = 6, Destination= GetTestPoints()[5] },
                new Route { Name = "teste", Time = 3, Cost = 5, OriginId = 5, Origin= GetTestPoints()[4], DestinationId = 4, Destination= GetTestPoints()[3] },
                new Route { Name = "teste", Time = 45, Cost = 50, OriginId = 6, Origin= GetTestPoints()[5], DestinationId = 9, Destination= GetTestPoints()[8] },
                new Route { Name = "teste", Time = 40, Cost = 50, OriginId = 6, Origin= GetTestPoints()[5], DestinationId = 7, Destination= GetTestPoints()[6] },
                new Route { Name = "teste", Time = 64, Cost = 73, OriginId = 7, Origin= GetTestPoints()[6], DestinationId = 2, Destination= GetTestPoints()[1] },
                new Route { Name = "teste", Time = 30, Cost = 1, OriginId = 8, Origin= GetTestPoints()[7], DestinationId = 5, Destination= GetTestPoints()[4] },
                new Route { Name = "teste", Time = 65, Cost = 5, OriginId = 9, Origin= GetTestPoints()[8], DestinationId = 2, Destination= GetTestPoints()[1] }
            });
            return routes;
        }

    }
}
