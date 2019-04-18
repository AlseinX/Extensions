using System.Reflection;

namespace Alsein.Extensions.Runtime.ProxyInvokers
{
    /// <summary>
    /// 
    /// </summary>
    public class WrapperProxyInvoker : IProxyInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        public IProxyInvoker Target { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public WrapperProxyInvoker(IProxyInvoker target) => Target = target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IArguments Invoke(MethodInfo method, IArguments args) => Target.Invoke(method, args);
    }
}
