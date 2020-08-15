using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RaduOrderBook.Comparers
{
    public class PriceComparer : Comparer<Tuple<string, decimal>>
    {

        public override int Compare([AllowNull] Tuple<string, decimal> x, [AllowNull] Tuple<string, decimal> y)
        {
            if (x != null && y != null)
                return Math.Sign(x.Item2 - y.Item2);
            return 0;
        }
    }
}
