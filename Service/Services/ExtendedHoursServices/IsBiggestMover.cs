namespace Service.Services.ExtendedHoursServices
{
    public class IsBiggestMover : IIsBiggestMover
    {
        const int POSITIVE_GAIN_PERCENT_THRESHOLD = 7;

        public bool Is(decimal closePrice, decimal afterHoursPrice)
        {
            var priceChange = afterHoursPrice - closePrice;
            var percentChanged = (priceChange / closePrice) * 100;

            return (percentChanged > POSITIVE_GAIN_PERCENT_THRESHOLD);
        }
    }

    public interface IIsBiggestMover
    {
        bool Is(decimal closePrice, decimal afterHoursPrice);
    }
}
