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
    public class PointsControllerTest
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfPoints()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.GetPoints()).Returns(Task.FromResult(GetTestPoints()));
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Point>>(viewResult.ViewData.Model);
            Assert.Equal(GetTestPoints().Count, model.Count());
        }

        [Fact]
        public async Task DetailsGet_ReturnsAViewResult_WithPoint()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.GetPointByID(1)).Returns(Task.FromResult(GetTestPoints()[0]));
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Point>(viewResult.ViewData.Model);
            Assert.Equal(GetTestPoints()[0], model);
        }

        [Fact]
        public async Task DetailsGet_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DetailsGet_ReturnsNotFound_WhenPointReturnedIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.GetPointByID(1)).Returns(Task.FromResult((Point) null));
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateGet_ReturnsAnEmptyViewResult()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CreatePost_ReturnsAViewResult_WithPoint_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            var controller = new PointsController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Required");
            var newPoint = new Point();

            // Act
            var result = await controller.Create(newPoint);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Point>(
                viewResult.ViewData.Model);
            Assert.Equal(newPoint, model);
        }

        [Fact]
        public async Task CreatePost_ReturnsARedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.InsertPoint(It.IsAny<Point>())).Verifiable();
            mockRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            var controller = new PointsController(mockRepo.Object);
            var newPoint = GetTestPoints()[0];

            // Act
            var result = await controller.Create(newPoint);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }

        [Fact]
        public async Task EditGet_ReturnsAViewResult_WithPoint()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.GetPointByID(1)).Returns(Task.FromResult(GetTestPoints()[0]));
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Point>(viewResult.ViewData.Model);
            Assert.Equal(GetTestPoints()[0], model);
        }

        [Fact]
        public async Task EditGet_ReturnsANotFoundResult_WhenIdIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsANotFoundResult_WhenPointReturnedIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.GetPointByID(1)).Returns(Task.FromResult((Point)null));
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task EditPost_ReturnsANotFoundResult_WhenIdIsDifferentFromPointId()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Edit(2, GetTestPoints()[0]);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsAViewResult_WithPoint_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            var controller = new PointsController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Required");
            var newPoint = new Point() { Id = 1 };

            // Act
            var result = await controller.Edit(newPoint.Id, newPoint);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Point>(
                viewResult.ViewData.Model);
            Assert.Equal(newPoint, model);
        }

        [Fact]
        public async Task EditPost_ReturnsARedirectToActionResult_WhenModelStateIsValid_AndNoErrorsHappenSaving()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.UpdatePoint(It.IsAny<Point>())).Verifiable();
            mockRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            var controller = new PointsController(mockRepo.Object);
            var newPoint = GetTestPoints()[0];

            // Act
            var result = await controller.Edit(newPoint.Id, newPoint);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }

        [Fact]
        public async Task EditPost_ReturnsANotFoundResult_WhenModelStateIsValid_AndErrorsHappenSaving_AndPointDoesNotExist()
        {
            // Arrange
            List<IUpdateEntry> entries = new List<IUpdateEntry>
            {
                null
            };
            string message = "Error message";
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.UpdatePoint(It.IsAny<Point>())).Throws(new DbUpdateConcurrencyException(message, entries));
            mockRepo.Setup(repo => repo.GetPointByID(It.IsAny<int>())).Returns(Task.FromResult((Point)null));
            var controller = new PointsController(mockRepo.Object);
            var newPoint = GetTestPoints()[0];

            // Act
            var result = await controller.Edit(newPoint.Id, newPoint);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ThrowsError_WhenModelStateIsValid_AndErrorsHappenSaving_AndPointExists()
        {
            // Arrange
            List<IUpdateEntry> entries = new List<IUpdateEntry>
            {
                null
            };
            string message = "Error message";
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.UpdatePoint(It.IsAny<Point>())).Throws(new DbUpdateConcurrencyException(message, entries));
            mockRepo.Setup(repo => repo.GetPointByID(It.IsAny<int>())).Returns(Task.FromResult(GetTestPoints()[0]));
            var controller = new PointsController(mockRepo.Object);
            var newPoint = GetTestPoints()[0];

            // Act
            var ex = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => controller.Edit(newPoint.Id, newPoint));

            // Assert
            Assert.Equal(message, ex.Message);
        }
        
        [Fact]
        public async Task DeleteGet_ReturnsAViewResult_WithPoint()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.GetPointByID(1)).Returns(Task.FromResult(GetTestPoints()[0]));
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Point>(viewResult.ViewData.Model);
            Assert.Equal(GetTestPoints()[0], model);
        }

        [Fact]
        public async Task DeleteGet_ReturnsANotFoundResult_WhenIdIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGet_ReturnsANotFoundResult_WhenPointReturnedIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.GetPointByID(1)).Returns(Task.FromResult((Point)null));
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async Task DeletePost_ReturnsARedirectToActionResult()
        {
            // Arrange
            var mockRepo = new Mock<IPointsRepository>();
            mockRepo.Setup(repo => repo.DeletePoint(It.IsAny<int>())).Verifiable();
            mockRepo.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            var controller = new PointsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify();
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
        
    }
}
