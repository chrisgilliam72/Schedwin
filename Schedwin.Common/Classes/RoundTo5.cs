using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Common
{
    public class RoundTo5
    {
        public static double Round(int originalVal)
        {
            int retVal = originalVal;

            int outResult;
            var modVal = Math.DivRem(originalVal, 5, out outResult);
            modVal = (modVal + 1) * 5;
            retVal += (modVal - originalVal);
            return retVal;
        }

    }
}
