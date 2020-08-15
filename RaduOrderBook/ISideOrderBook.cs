using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RaduOrderBook.Entities;
using RaduOrderBook.Static;

namespace RaduOrderBook
{
    public interface ISideOrderBook
    {

        Side Side { get; }
        //<orderId, Orders>
        public ConcurrentDictionary<string, Order> Orders { get; }
        //<ticker, bst with prices>
        public ConcurrentDictionary<string, SortedSet<Tuple<string, decimal>>> BestPrices { get; }
        public void Process(Order order);
        decimal GetBestPrice(string ticker);
    }
}
