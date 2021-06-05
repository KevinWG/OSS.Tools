using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OSS.Tools.DataFlow.Inter
{
    public static class InterUtils
    {
        public static Task<bool> TrueTask => Task.FromResult(true);
    }
}
