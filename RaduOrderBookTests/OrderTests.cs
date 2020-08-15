using System;
using RaduOrderBook.Entities;
using RaduOrderBook.Exceptions;
using RaduOrderBook.Static;
using Xunit;

namespace RaduOrderBookTests
{
    public class OrderTests
    {
        [Fact]
        public void Order_Is_Correctly_Updated()
        {
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order update = new Order(1597508855, "ab1", ActionType.UPDATE, 120);
            order.UpdateOrder(update);

            Assert.Equal(120, order.GetLastOrder().Size);
        }

        [Fact]
        public void Order_Is_Correctly_Cancelled_After_Update()
        {
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order update = new Order(1597508855, "ab1", ActionType.UPDATE, 120);
            Order cancel = new Order(1597508855, "ab1", ActionType.CANCEL);
            order.UpdateOrder(update);
            order.CancelOrder(cancel);

            Assert.Equal(ActionType.CANCEL, order.GetLastOrder().ActionType);
        }

        [Fact]
        public void Order_Is_Correctly_Cancelled()
        {
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order cancel = new Order(1597508855, "ab1", ActionType.CANCEL);
            order.CancelOrder(cancel);

            Assert.Equal(ActionType.CANCEL, order.GetLastOrder().ActionType);
        }

        [Fact]
        public void Updating_Cancelled_Order_Should_Throw_Exception()
        {
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order cancel = new Order(1597508855, "ab1", ActionType.CANCEL);
            Order update = new Order(1597508855, "ab1", ActionType.UPDATE, 120);
            order.CancelOrder(cancel);
            Action act = () => order.UpdateOrder(update);
            Assert.Throws<UpdateException>(act);
        }

        [Fact]
        public void Cancelling_Cancelled_Order_Should_Throw_Exception()
        {
            Order order = new Order(1597508855, "ab1", ActionType.ADD, "APPL", Side.ASK, 10, 100);
            Order cancel = new Order(1597508855, "ab1", ActionType.CANCEL);
            Order cancel2 = new Order(1597508855, "ab1", ActionType.CANCEL);
            order.CancelOrder(cancel);
            Action act = () => order.CancelOrder(cancel2);
            Assert.Throws<CancelException>(act);
        }
    }
}
