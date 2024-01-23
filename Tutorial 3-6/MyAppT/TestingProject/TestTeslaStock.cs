using MyAppT.Infrastructure;
using Xunit;

namespace TestingProject
{
    public class TestTeslaStock
    {
        [Fact]
        public void Test_Predict()
        {
            // Arrange     
            TeslaStock TS = new TeslaStock();

            // Act
            int newValue = TS.Predict(800);

            // Assert
            Assert.Equal(1200, newValue);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(400)]
        [InlineData(800)]
        public void Test_PredictMultiple(int currentValue)
        {
            // Arrange
            TeslaStock TS = new TeslaStock();

            // Act
            int newValue = TS.Predict(currentValue);
            int calculate = Convert.ToInt32(currentValue + (.5 * currentValue));

            // Assert
            Assert.Equal(currentValue + (.5 * currentValue), newValue);
        }
    }
}
