using Microsoft.AspNetCore.Mvc;
using Moq;
using MyAppT.Controllers;
using MyAppT.Infrastructure;
using MyAppT.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestingProject
{
    public class TestAPI
    {
        [Fact]
        public void Test_GET_AllReservations()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Reservations).Returns(Multiple());
            var controller = new ReservationController(mockRepo.Object);

            // Act
            var result = controller.Get();

            // Assert
            var model = Assert.IsAssignableFrom<IEnumerable<Reservation>>(result);
            Assert.Equal(3, model.Count());
        }
        private static IEnumerable<Reservation> Multiple()
        {
            var r = new List<Reservation>();
            r.Add(new Reservation()
            {
                Id = 1,
                Name = "Test One",
                StartLocation = "SL1",
                EndLocation = "EL1"
            });
            r.Add(new Reservation()
            {
                Id = 2,
                Name = "Test Two",
                StartLocation = "SL2",
                EndLocation = "EL2"
            });
            r.Add(new Reservation()
            {
                Id = 3,
                Name = "Test Three",
                StartLocation = "SL3",
                EndLocation = "EL3"
            });
            return r;
        }

        [Fact]
        public void Test_GET_AReservations_BadRequest()
        {
            // Arrange
            int id = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((a) => Single(a));
            var controller = new ReservationController(mockRepo.Object);

            // Act
            var result = controller.Get(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        private static Reservation Single(int id)
        {
            IEnumerable<Reservation> reservations = Multiple();
            return reservations.Where(a => a.Id == id).FirstOrDefault();
        }

        [Fact]
        public void Test_GET_AReservations_Ok()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((id) => Single(id));
            var controller = new ReservationController(mockRepo.Object);

            // Act
            var result = controller.Get(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            var actionValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(id, ((Reservation)actionValue.Value).Id);
        }

        [Fact]
        public void Test_GET_AReservations_NotFound()
        {
            // Arrange
            int id = 4;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((id) => Single(id));
            var controller = new ReservationController(mockRepo.Object);

            // Act
            var result = controller.Get(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void Test_POST_AddReservation()
        {
            // Arrange
            Reservation r = new Reservation()
            {
                Id = 4,
                Name = "Test Four",
                StartLocation = "SL4",
                EndLocation = "EL4"
            };
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.AddReservation(It.IsAny<Reservation>())).Returns(r);
            var controller = new ReservationController(mockRepo.Object);

            // Act
            var result = controller.Post(r);

            // Assert
            var reservation = Assert.IsType<Reservation>(result);
            Assert.Equal(4, reservation.Id);
            Assert.Equal("Test Four", reservation.Name);
            Assert.Equal("SL4", reservation.StartLocation);
            Assert.Equal("EL4", reservation.EndLocation);
        }

        [Fact]
        public void Test_PUT_UpdateReservation()
        {
            // Arrange
            Reservation r = new Reservation()
            {
                Id = 3,
                Name = "new name",
                StartLocation = "new start location",
                EndLocation = "new end location"
            };
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.UpdateReservation(It.IsAny<Reservation>())).Returns(r);
            var controller = new ReservationController(mockRepo.Object);

            // Act
            var result = controller.Put(r);

            // Assert
            var reservation = Assert.IsType<Reservation>(result);
            Assert.Equal(3, reservation.Id);
            Assert.Equal("new name", reservation.Name);
            Assert.Equal("new start location", reservation.StartLocation);
            Assert.Equal("new end location", reservation.EndLocation);
        }

        [Fact]
        public void Test_DELETE_Reservation()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.DeleteReservation(It.IsAny<int>())).Verifiable();
            var controller = new ReservationController(mockRepo.Object);

            // Act
            controller.Delete(3);

            // Assert
            mockRepo.Verify();
        }
    }
}
