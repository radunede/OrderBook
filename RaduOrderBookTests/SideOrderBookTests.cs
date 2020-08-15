using System;
using RaduOrderBook;
using RaduOrderBook.Entities;
using RaduOrderBook.Static;
using Xunit;

namespace RaduOrderBookTests
{
    public class SideOrderBookTests
    {

        [Fact]
        public void OrderBook_Should_CorrectlyAddOrder()
        {
            ISideOrderBook orderBook = new SideOrderBook(Side.ASK);
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order update = new Order(1597508855, "ab1", ActionType.UPDATE, 120);
            orderBook.Process(order);
            orderBook.Process(update);
            Assert.Equal(10, orderBook.GetBestPrice("APPL"));
        }

        [Fact]
        public void OrderBook_Should_Correctly_Return_Best_Ask_Price()
        {
            ISideOrderBook orderBook = new SideOrderBook(Side.ASK);
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 12, 100);
            Order update = new Order(1597508855, "ab1", ActionType.UPDATE, 120);
            Order order2 = new Order(1597508855, "ab2", ActionType.ADD, "APPL", Side.ASK, 10, 50);
            orderBook.Process(order);
            orderBook.Process(update);
            orderBook.Process(order2);
            Assert.Equal(10, orderBook.GetBestPrice("APPL"));
        }

        [Fact]
        public void OrderBook_Should_Correctly_Return_Best_Bid_Price()
        {
            ISideOrderBook orderBook = new SideOrderBook(Side.BID);
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order update = new Order(1597508855, "ab1", ActionType.UPDATE, 120);
            Order order2 = new Order(1597508855, "ab2", ActionType.ADD, "APPL", Side.ASK, 15, 50);
            orderBook.Process(order);
            orderBook.Process(update);
            orderBook.Process(order2);
            Assert.Equal(15, orderBook.GetBestPrice("APPL"));
        }

        [Fact]
        public void OrderBook_Should_Correctly_Return_Best_Bid_Price_After_Cancellation()
        {
            ISideOrderBook orderBook = new SideOrderBook(Side.BID);
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order update = new Order(1597508855, "ab1", ActionType.UPDATE, 120);
            Order order2 = new Order(1597508855, "ab2", ActionType.ADD, "APPL", Side.ASK, 15, 50);
            Order order3 = new Order(1597508855, "ab2", ActionType.CANCEL);
            orderBook.Process(order);
            orderBook.Process(update);
            orderBook.Process(order2);
            orderBook.Process(order3);
            Assert.Equal(10, orderBook.GetBestPrice("APPL"));
        }
    }
}

