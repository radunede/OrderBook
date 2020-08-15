using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RaduOrderBook.Comparers;
using RaduOrderBook.Entities;
using RaduOrderBook.Static;

namespace RaduOrderBook
{
    public class SideOrderBook : ISideOrderBook
    {
        public SideOrderBook(Side side)
        {
            Orders = new ConcurrentDictionary<string, Order>();
            BestPrices = new ConcurrentDictionary<string, SortedSet<Tuple<string, decimal>>>();
            Side = side;
        }

        public Side Side { get;}
        // <orderId, Order>
        public ConcurrentDictionary<string, Order> Orders { get; }
        // <ticker, <orderid, price>>
        public ConcurrentDictionary<string, SortedSet<Tuple<string, decimal>>> BestPrices { get; }

        public decimal GetBestPrice(string ticker)
        {
            if (BestPrices.ContainsKey(ticker))
                return this.Side == Side.BID ? BestPrices[ticker].Max.Item2 : BestPrices[ticker].Min.Item2;
            else return 0;
        }

        public void Process(Order order)
        {
            switch (order.ActionType)
            {
                case ActionType.ADD:
                    {
                        Orders.TryAdd(order.OrderId, order);
                        if (BestPrices.ContainsKey(order.Ticker))
                        {
                            BestPrices[order.Ticker].Add(Tuple.Create(order.OrderId, order.Price));
                        }
                        else
                        {
                            var bst = new SortedSet<Tuple<string, decimal>>(new PriceComparer());
                            bst.Add(Tuple.Create(order.OrderId, order.Price));
                            BestPrices.TryAdd(order.Ticker, bst);
                        }
                        break;
                    }
                case ActionType.UPDATE:
                    {
                        if (Orders.ContainsKey(order.OrderId))
                            Orders[order.OrderId].UpdateOrder(order);
                        else
                            throw new Exception($"Cannot update the order id {order.OrderId} because it does not exist in {Side} book");
                        break;
                    }
                case ActionType.CANCEL:
                    {
                        if (Orders.ContainsKey(order.OrderId))
                        {
                            Orders[order.OrderId].CancelOrder(order);
                            BestPrices[order.Ticker].RemoveWhere(t => t.Item1 == order.OrderId);
                            if (BestPrices[order.Ticker].Count == 0)
                                BestPrices.TryRemove(order.Ticker, out _);
                        }
                        else
                            throw new Exception($"Cannot cancel the order id {order.OrderId} because it does not exist in {Side} book");
                        break;
                    }
            }

        }
    }
}
