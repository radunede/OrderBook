using System;
using RaduOrderBook;
using RaduOrderBook.Entities;
using RaduOrderBook.Exceptions;
using RaduOrderBook.Static;
using Xunit;

namespace RaduOrderBookTests
{
    public class OrderBookTests
    {
        [Fact]
        public void Cancelling_An_Order_Should_Return_No_Best_Price()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|B|209.00|100";
            string order2 = "1568390244|abb11|u|101";
            string order3 = "1568390244|abb11|c";

            clientBookOrderApi.Process(order1);
            clientBookOrderApi.Process(order2);
            clientBookOrderApi.Process(order3);

            Assert.Equal(Tuple.Create(0m, 0m), clientBookOrderApi.GetBestBidAndAsk("AAPL"));
        }

        [Fact]
        public void Should_Return_Best_Bid_Price()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|B|209.00|100";
            string order2 = "1568390244|abb11|u|101";
            string order4 = "1568390243|abb12|a|AAPL|B|208.75|100";

            clientBookOrderApi.Process(order1);
            clientBookOrderApi.Process(order2);
            clientBookOrderApi.Process(order4);


            Assert.Equal(Tuple.Create(209m, 0m), clientBookOrderApi.GetBestBidAndAsk("AAPL"));
        }

        [Fact]
        public void Should_Return_Best_Ask_Price()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|S|209.00|100";
            string order2 = "1568390244|abb11|u|101";
            string order4 = "1568390243|abb12|a|AAPL|S|208.75|100";

            clientBookOrderApi.Process(order1);
            clientBookOrderApi.Process(order2);
            clientBookOrderApi.Process(order4);


            Assert.Equal(Tuple.Create(0m, 208.75m), clientBookOrderApi.GetBestBidAndAsk("AAPL"));
        }

        [Fact]
        public void Cancelling_And_Updating_Multiple_Orders_For_Same_Ticker_Should_Return_Best_Bid_And_Ask_Price()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|B|209.00|100";
            string order2 = "1568390244|abb11|u|101";
            string order3 = "1568390244|abb11|c";
            string order4 = "1568390246|abb12|a|AAPL|B|230.00|100";
            string order5 = "1568390246|abb13|a|AAPL|B|220.00|100";
            string order6 = "1568390247|abb14|a|AAPL|S|250.00|50";
            string order7 = "1568390248|abb15|a|AAPL|S|230.00|50";
            string order8 = "1568390244|abb12|c";

            clientBookOrderApi.Process(order1);
            clientBookOrderApi.Process(order2);
            clientBookOrderApi.Process(order3);
            clientBookOrderApi.Process(order4);
            clientBookOrderApi.Process(order5);
            clientBookOrderApi.Process(order6);
            clientBookOrderApi.Process(order7);
            clientBookOrderApi.Process(order8);


            Assert.Equal(Tuple.Create(220m, 230m), clientBookOrderApi.GetBestBidAndAsk("AAPL"));
        }

        [Fact]
        public void Processing_Multiple_Orders_For_Same_Ticker_Should_Return_Best_Bid_And_Ask_Price()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|B|209.00|100";
            string order2 = "1568390244|abb11|u|101";
            string order3 = "1568390244|abb11|c";

            string order4 = "1568390246|abb12|a|AAPL|B|230.00|100";
            string order5 = "1568390246|abb13|a|AAPL|B|301.24321|100";
            string order6 = "1568390247|abb14|a|AAPL|S|250.00|50";

            string order7 = "1568390248|abb15|a|AAPL|S|230.00|50";
            string order8 = "1568390249|abb15|c";

            string order9 =  "1568390243|abb16|a|AAPL|B|301.24322|100";
            string order10 = "1568390247|abb17|a|AAPL|S|249.01|50";
            string order11 = "1568390252|abb17|u|100";

            clientBookOrderApi.Process(order1);
            clientBookOrderApi.Process(order2);
            clientBookOrderApi.Process(order3);
            clientBookOrderApi.Process(order4);
            clientBookOrderApi.Process(order5);
            clientBookOrderApi.Process(order6);
            clientBookOrderApi.Process(order7);
            clientBookOrderApi.Process(order8);
            clientBookOrderApi.Process(order9);
            clientBookOrderApi.Process(order10);
            clientBookOrderApi.Process(order11);



            Assert.Equal(Tuple.Create(301.24322m, 249.01m), clientBookOrderApi.GetBestBidAndAsk("AAPL"));
        }


        [Fact]
        public void Processing_Order_String_With_Bad_Action_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|d|AAPL|B|209.00|100";

            Action act = () => clientBookOrderApi.Process(order1);
   

            Assert.Throws<OrderParserException>(act);
        }

        [Fact]
        public void Processing_Order_String_With_Bad_Side_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|C|209.00|100";

            Action act = () => clientBookOrderApi.Process(order1);


            Assert.Throws<OrderParserException>(act);
        }

        [Fact]
        public void Processing_Order_String_With_Bad_Price_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|B||100";

            Action act = () => clientBookOrderApi.Process(order1);


            Assert.Throws<OrderParserException>(act);
        }

        [Fact]
        public void Processing_Order_String_With_Bad_Size_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|B|100|asd";

            Action act = () => clientBookOrderApi.Process(order1);


            Assert.Throws<OrderParserException>(act);
        }

        [Fact]
        public void Processing_Incomplete_Order_String_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|a|AAPL|B";

            Action act = () => clientBookOrderApi.Process(order1);


            Assert.Throws<OrderParserException>(act);
        }

        [Fact]
        public void Processing_Cancel_Order_String_With_Extra_Fields_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|C|AAPL";

            Action act = () => clientBookOrderApi.Process(order1);


            Assert.Throws<OrderParserException>(act);
        }

        [Fact]
        public void Processing_Update_Order_String_With_Extra_Fields_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|u|AAPL|B|209.00|100";

            Action act = () => clientBookOrderApi.Process(order1);


            Assert.Throws<OrderParserException>(act);
        }

        [Fact]
        public void Processing_New_Order_String_With_0_Size_Should_Throw_Exception()
        {
            IOrderBook clientBookOrderApi = new OrderBook();
            string order1 = "1568390243|abb11|u|AAPL|B|209.00|0";

            Action act = () => clientBookOrderApi.Process(order1);


            Assert.Throws<OrderParserException>(act);
        }
    }
}
