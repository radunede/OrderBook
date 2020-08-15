using System;
namespace RaduOrderBook
{
    public interface IOrderBook
    {
        void Process(string order);
        Tuple<decimal, decimal> GetBestBidAndAsk(string ticker);
    }
}
