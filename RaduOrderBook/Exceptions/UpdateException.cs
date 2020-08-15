using System;
namespace RaduOrderBook.Exceptions
{
    public class UpdateException : Exception
    {
        public UpdateException(string message) : base(message) 
        {
        }

    }
}
