using System.Threading.Tasks;

namespace OSS.Tools.DataFlow
{
    public static class InterUtils
    {
        public static Task<bool> TrueTask => Task.FromResult(true);
    }
}
