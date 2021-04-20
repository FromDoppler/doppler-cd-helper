using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.CDHelper
{
    public static class AssertHelper
    {
        public static bool GetValueAndContinue<T>(T input, out T output)
        {
            output = input;
            return true;
        }

}
