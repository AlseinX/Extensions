using System.Reflection;

namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReflectionInvoker
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