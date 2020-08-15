using System;
namespace RaduOrderBook.Exceptions
{
    public class OrderParserException : Exception
    {
        public OrderParserException(string message) : base(message)
        {
        }
    }
}
