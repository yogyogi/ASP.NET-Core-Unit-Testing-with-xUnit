using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAppT.Controllers;
using MyAppT.Models;
using Xunit;

namespace TestingProject
{
    public abstract class TestRegistration
    {
        #region Seeding
        public TestRegistration(DbContextOptions<AppDbContext> contextOptions)
        {
            ContextOptions = contextOptions;
            Seed();
        }

        protected DbContextOptions<AppDbContext> ContextOptions { get; }

        private void Seed()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var one = new Register()
                {
                    Name = "Test One",
                    Age = 40
                };

                var two = new Register()
                {
                    Name = "Test Two",
                    Age = 50
                };

                var three = new Register()
                {
                    Name = "Test Three",
                    Age = 60
                };
                context.AddRange(one, two, three);
                context.SaveChanges();
            }
        }
        #endregion

        [Fact]
        public void Test_Create_GET_ReturnsViewResultNullModel()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                var controller = new RegistrationController(context);

                // Act
                var result = controller.Create();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Null(viewResult.ViewData.Model);
            }
        }

        [Fact]
        public async Task Test_Create_POST_InvalidModelState()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                var r = new Register()
                {
                    Id = 4,
                    Name = "Test Four",
                    Age = 59
                };
                var controller = new RegistrationController(context);
                controller.ModelState.AddModelError("Name", "Name is required");

                // Act
                var result = await controller.Create(r);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Null(viewResult.ViewData.Model);
            }
        }

        [Fact]
        public async Task Test_Create_POST_ValidModelState()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                var r = new Register()
                {
                    Id = 4,
                    Name = "Test Four",
                    Age = 59
                };

                var controller = new RegistrationController(context);

                // Act
                var result = await controller.Create(r);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Null(redirectToActionResult.ControllerName);
                Assert.Equal("Read", redirectToActionResult.ActionName);
            }
        }

        [Fact]
        public void Test_Read_GET_ReturnsViewResult_WithAListOfRegistrations()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                var controller = new RegistrationController(context);

                // Act
                var result = controller.Read();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Register>>(viewResult.ViewData.Model);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public void Test_Update_GET_ReturnsViewResult_WithSingleRegistration()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                int testId = 2;

                var controller = new RegistrationController(context);

                // Act
                var result = controller.Update(testId);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Register>(viewResult.ViewData.Model);
                Assert.Equal(testId, model.Id);
                Assert.Equal("Test Two", model.Name);
                Assert.Equal(50, model.Age);
            }
        }

        [Fact]
        public async Task Test_Update_POST_ReturnsViewResult_InValidModelState()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                int testId = 2;
                var r = new Register()
                {
                    Id = 2,
                    Name = "Test Four",
                    Age = 59
                };
                var controller = new RegistrationController(context);
                controller.ModelState.AddModelError("Name", "Name is required");

                // Act
                var result = await controller.Update(r);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Register>(viewResult.ViewData.Model);
                Assert.Equal(testId, model.Id);
            }
        }

        [Fact]
        public async Task Test_Update_POST_ReturnsViewResult_ValidModelState()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                int testId = 2;
                var r = new Register()
                {
                    Id = 2,
                    Name = "Test Four",
                    Age = 59
                };
                var controller = new RegistrationController(context);

                // Act
                var result = await controller.Update(r);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Register>(viewResult.ViewData.Model);
                Assert.Equal(testId, model.Id);
                Assert.Equal(r.Name, model.Name);
                Assert.Equal(r.Age, model.Age);
            }
        }

        [Fact]
        public async Task Test_Delete_POST_ReturnsViewResult_InValidModelState()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                // Arrange
                int testId = 2;

                var controller = new RegistrationController(context);

                // Act
                var result = await controller.Delete(testId);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Null(redirectToActionResult.ControllerName);
                Assert.Equal("Read", redirectToActionResult.ActionName);
            }
        }
    }
}
