using System;
namespace RaduOrderBook.Exceptions
{
    public class CancelException : Exception
    {
        public CancelException(string message) : base(message)
        {
        }
    }
}
