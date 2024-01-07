namespace MyAppT.Infrastructure
{
    public class TeslaStock
    {
        public int Predict(int currentValue)
        {
            int newValue = Convert.ToInt32(currentValue + (.5 * currentValue));
            return newValue;
        }
    }
}
