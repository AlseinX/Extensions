using System.Reflection;

namespace Alsein.Utilities.Runtime.ReflectionInvokers
{
    /// <summary>
    /// 
    /// </summary>
    public class WrapperReflectionInvoker : IReflectionInvoker
    {
        private readonly IReflectionInvoker _target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public WrapperReflectionInvoker(IReflectionInvoker target) => _target = target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Invoke(MethodInfo method, params object[] args) => _target.Invoke(method, args);
    }
}
