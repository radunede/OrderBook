using System;
using System.ComponentModel;

namespace RaduOrderBook.Static
{
    public enum Side
    {
        ERROR,
        [Description("B")]
        BID,
        [Description("S")]
        ASK
    }
}
