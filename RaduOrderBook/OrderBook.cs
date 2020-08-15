using System;
using System.Collections.Concurrent;
using System.Linq;
using RaduOrderBook.Entities;
using RaduOrderBook.Exceptions;
using RaduOrderBook.Static;

namespace RaduOrderBook
{
    public class OrderBook : IOrderBook
    {
        private ISideOrderBook _bids;
        private ISideOrderBook _asks;
        private ConcurrentDictionary<string, ISideOrderBook> _orderIdBookMapping;
        public OrderBook()
        {
            _bids = new SideOrderBook(Side.BID);
            _asks = new SideOrderBook(Side.ASK);
            _orderIdBookMapping = new ConcurrentDictionary<string, ISideOrderBook>();
        }

        public Tuple<decimal, decimal> GetBestBidAndAsk(string ticker)
        {
            decimal maxBid = _bids.GetBestPrice(ticker);
            decimal maxAsk = _asks.GetBestPrice(ticker);
            return Tuple.Create(maxBid, maxAsk);
        }

        private void Process(long timestamp, string orderId, ActionType actionType, string ticker,Side side,decimal price, int size)
        {
            
            try
            {
                Order newOrder = null;
                switch (actionType)
                {
                    case ActionType.ADD:
                        {
                            newOrder = new Order(timestamp, orderId, actionType, ticker, side, price, size);
                            if (!_orderIdBookMapping.ContainsKey(orderId))
                                _orderIdBookMapping.TryAdd(orderId, newOrder.Side == Side.ASK ? _asks : _bids);
                            _orderIdBookMapping[orderId].Process(newOrder);
                            ; break;
                        }
                    case ActionType.UPDATE:
                        {
                            newOrder = new Order(timestamp, orderId, actionType, size);
                            _orderIdBookMapping[orderId].Process(newOrder);

                            break;
                        }
                    case ActionType.CANCEL:
                        {
                            newOrder = new Order(timestamp, orderId, actionType);
                            if (_orderIdBookMapping.ContainsKey(orderId))
                            {
                                _orderIdBookMapping[orderId].Process(newOrder);
                                _orderIdBookMapping.TryRemove(orderId, out _);
                            }
                            break;
                        }
                } 
                
            }
            catch (Exception e)
            {
                throw new OrderParserException($"Unable to parse order. Message:{e.Message}");
            }
        }

        public void Process(string order)
        {
            string[] split = order.Split('|', StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 3)
                throw new OrderParserException("Incomplete order string");
            long timestamp = long.Parse(split[0]);
            string orderId = split[1];
            ActionType actionType = Helper.GetValueFromDescription<ActionType>(split[2]);
            string ticker = string.Empty;
            Side side = Side.ERROR;
            decimal price = decimal.Zero;
            int size = 0;
            if (actionType == ActionType.ERROR)
                throw new OrderParserException($"Unable to parse action type. Unidentifiable character {split[2]}");

            if (actionType == ActionType.ADD)
            {
                if (split.Length != 7)
                    throw new OrderParserException("Order string has bad format");
                ticker = split[3];
                side = Helper.GetValueFromDescription<Side>(split[4]);
                if (side == Side.ERROR)
                    throw new OrderParserException($"Unable to parse side: {split[4]}");

                if(!decimal.TryParse(split[5], out price))
                    throw new OrderParserException($"Unable to parse price: {split[5]}");
                if (!int.TryParse(split[6], out size))
                    throw new OrderParserException($"Unable to parse size: {split[6]}");
                if (size <= 0)
                    throw new OrderParserException($"Size cannot be less or equal to 0: {size}");

            }
            if (actionType == ActionType.UPDATE)
            {
                if (split.Length != 4)
                    throw new OrderParserException($"Order string has bad format");
                if (!int.TryParse(split[3], out size))
                    throw new OrderParserException($"Unable to parse size: {split[3]}");

            }
            if (actionType == ActionType.CANCEL)
            {
                if (split.Length != 3)
                    throw new OrderParserException($"Order string has bad format");
            }

            Process(timestamp, orderId, actionType, ticker, side, price, size);
        }
    }
}
