using Microsoft.AspNetCore.Mvc;
using Moq;
using MyAppT.Controllers;
using MyAppT.Models;
using Xunit;

namespace TestingProject
{
    public class TestRegister
    {
        [Fact]
        public void Test_Create_GET_ReturnsViewResultNullModel()
        {
            // Arrange
            IRegisterRepository context = null;
            var controller = new RegisterController(context);

            // Act
            var result = controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Test_Create_POST_InvalidModelState()
        {
            // Arrange
            var r = new Register()
            {
                Id = 4,
                Name = "Test Four",
                Age = 59
            };
            var mockRepo = new Mock<IRegisterRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Register>()));
            var controller = new RegisterController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await controller.Create(r);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
            mockRepo.Verify();
        }

        [Fact]
        public async Task Test_Create_POST_ValidModelState()
        {
            // Arrange
            var r = new Register()
            {
                Id = 4,
                Name = "Test Four",
                Age = 59
            };

            var mockRepo = new Mock<IRegisterRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Register>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var controller = new RegisterController(mockRepo.Object);

            // Act
            var result = await controller.Create(r);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Read", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }

        [Fact]
        public async Task Test_Read_GET_ReturnsViewResult_WithAListOfRegistrations()
        {
            // Arrange
            var mockRepo = new Mock<IRegisterRepository>();
            mockRepo.Setup(repo => repo.ListAsync()).ReturnsAsync(GetTestRegistrations());
            var controller = new RegisterController(mockRepo.Object);

            // Act
            var result = await controller.Read();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Register>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        private static List<Register> GetTestRegistrations()
        {
            var registrations = new List<Register>
            {
                new Register()
                {
                    Id = 1,
                    Name = "Test One",
                    Age = 45
                },
                new Register()
                {
                    Id = 2,
                    Name = "Test Two",
                    Age = 55
                },
                new Register()
                {
                    Id = 3,
                    Name = "Test Three",
                    Age = 60
                }
            };
            return registrations;
        }

        [Fact]
        public async Task Test_Update_GET_ReturnsViewResult_WithSingleRegistration()
        {
            // Arrange
            int testId = 2;
            string testName = "test name";
            int testAge = 60;

            var mockRepo = new Mock<IRegisterRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestRegisterRecord());
            var controller = new RegisterController(mockRepo.Object);

            // Act
            var result = await controller.Update(testId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Register>(viewResult.ViewData.Model);
            Assert.Equal(testId, model.Id);
            Assert.Equal(testName, model.Name);
            Assert.Equal(testAge, model.Age);
        }

        private Register GetTestRegisterRecord()
        {
            var r = new Register()
            {
                Id = 2,
                Name = "test name",
                Age = 60
            };
            return r;
        }

        [Fact]
        public async Task Test_Update_POST_ReturnsViewResult_InValidModelState()
        {
            // Arrange
            int testId = 2;
            Register r = GetTestRegisterRecord();

            var mockRepo = new Mock<IRegisterRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestRegisterRecord());

            var controller = new RegisterController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await controller.Update(r);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Register>(viewResult.ViewData.Model);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async Task Test_Update_POST_ReturnsViewResult_ValidModelState()
        {
            // Arrange
            int testId = 2;
            var r = new Register()
            {
                Id = 2,
                Name = "Test Two",
                Age = 55
            };
            var mockRepo = new Mock<IRegisterRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(GetTestRegisterRecord());
            var controller = new RegisterController(mockRepo.Object);

            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Register>()))
                   .Returns(Task.CompletedTask)
                   .Verifiable();

            // Act
            var result = await controller.Update(r);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Register>(viewResult.ViewData.Model);
            Assert.Equal(testId, model.Id);
            Assert.Equal(r.Name, model.Name);
            Assert.Equal(r.Age, model.Age);

            mockRepo.Verify();
        }

        [Fact]
        public async Task Test_Delete_POST_ReturnsViewResult_InValidModelState()
        {
            // Arrange
            int testId = 2;

            var mockRepo = new Mock<IRegisterRepository>();
            mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                   .Returns(Task.CompletedTask)
                   .Verifiable();

            var controller = new RegisterController(mockRepo.Object);

            // Act
            var result = await controller.Delete(testId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Read", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }
    }
}
