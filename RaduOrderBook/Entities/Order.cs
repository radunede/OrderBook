using System;
using RaduOrderBook.Exceptions;
using RaduOrderBook.Static;

namespace RaduOrderBook.Entities
{
    public class Order
    {

        public Order(long timestamp, string orderId, ActionType actionType)
        {
            Timestamp = timestamp;
            OrderId = orderId;
            ActionType = actionType;
        }

        public Order(long timestamp, string orderId, ActionType actionType, int size)
        {
            Timestamp = timestamp;
            OrderId = orderId;
            ActionType = actionType;
            Size = size;
        }

        public Order(long timestamp, string orderId, ActionType actionType, string ticker, Side side, decimal price, int size)
        {
            Timestamp = timestamp;
            OrderId = orderId;
            ActionType = actionType;
            Ticker = ticker;
            Side = side;
            Price = price;
            Size = size;
        }

        private Order UpdatedOrder { get; set; }
        public long Timestamp { get; }
        public string OrderId { get; }
        public ActionType ActionType { get; }
        public string Ticker { get; set; }
        public Side Side { get; set; }
        public decimal Price { get; set; }
        public int Size { get; set; }

        public void UpdateOrder(Order order)
        {
            var lastOrder = GetLastOrder();
            if (lastOrder.ActionType == ActionType.CANCEL)
                throw new UpdateException($"Cannot update order {this.OrderId} because it was cancelled.");
            order.Ticker = this.Ticker;
            order.Side = this.Side;
            order.Price = this.Price;
            lastOrder.UpdatedOrder = order;
        }

        public void CancelOrder(Order order)
        {
            var lastOrder = GetLastOrder();
            if (lastOrder.ActionType == ActionType.CANCEL)
                throw new CancelException($"Cannot cancel order {this.OrderId} because it is already cancelled.");
            order.Ticker = this.Ticker;
            order.Side = this.Side;
            order.Price = this.Price;
            order.Size = this.Size;
            lastOrder.UpdatedOrder = order;
        }

        public Order GetLastOrder()
        {
            if (UpdatedOrder == null)
                return this;
            else
                return UpdatedOrder.GetLastOrder();
        }

    }
}
