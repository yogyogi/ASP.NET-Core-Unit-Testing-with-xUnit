using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyAppT.Controllers;

namespace TestingProject
{
    public class TestParty
    {
        [Fact]
        public void Test_Entry_GET_ReturnsViewResultNullModel()
        {
            // Arrange
            var controller = new PartyController();

            // Act
            var result = controller.Entry();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public void Test_Entry_POST_InvalidModelState()
        {
            // Arrange
            var controller = new PartyController();

            // Act
            var result = controller.Entry(null, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Test_Entry_POST_ValidModelState()
        {
            // Arrange
            string name = "Tom Cruise", membership = "Platinum";
            var controller = new PartyController();

            // Act
            var result = controller.Entry(name, membership);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<string>(viewResult.ViewData.Model);
        }
    }
}