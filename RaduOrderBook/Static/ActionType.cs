using System;
using System.ComponentModel;

namespace RaduOrderBook.Static
{
    public enum ActionType
    {
        ERROR,
        [Description("a")]
        ADD,
        [Description("u")]
        UPDATE,
        [Description("c")]
        CANCEL
    }
}
