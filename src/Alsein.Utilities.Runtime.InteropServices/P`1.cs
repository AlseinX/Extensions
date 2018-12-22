using System;
using System.Reflection;

namespace Alsein.Utilities.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFunc"></typeparam>
    public static class P<TFunc> where TFunc : Delegate
    {
        /// <summary>
        /// 
        /// </summary>
        public static TFunc Invoke
        {
            get
            {
                var tFunc = typeof(TFunc);
                return (TFunc)P.GetModule(tFunc.DeclaringType).GetFunction(tFunc.GetCustomAttribute<EntryPoint>()?.Value ?? tFunc.Name, tFunc);
            }
        }
    }
}
