using System;

namespace Alsein.Utilities
{
    public static class Utils
    {
        public static TResult EvaluateOrDefault<TResult>(this Func<TResult> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(TResult);
            }
        }
    }
}