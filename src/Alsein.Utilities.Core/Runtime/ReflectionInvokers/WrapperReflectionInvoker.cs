using System.Reflection;

namespace Alsein.Utilities.Runtime.ReflectionInvokers
{
    /// <summary>
    /// 
    /// </summary>
    public class WrapperReflectionInvoker : IReflectionInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        public IReflectionInvoker Target { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public WrapperReflectionInvoker(IReflectionInvoker target) => Target = target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IArguments Invoke(MethodInfo method, IArguments args) => Target.Invoke(method, args);
    }
}
