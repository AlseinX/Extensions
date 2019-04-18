using System.Reflection;

namespace Alsein.Extensions.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        IArguments Invoke(MethodInfo method, IArguments args);
    }
}