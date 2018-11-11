using System;
using System.Linq;
using System.Reflection;

namespace Alsein.Utilities.Runtime.DynamicInvokers
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectDynamicInvoker : IDynamicInvoker
    {
        private readonly object _target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public ObjectDynamicInvoker(object target) => _target = target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="genericArgs"></param>
        /// <param name="valueArgs"></param>
        /// <returns></returns>
        public object InvokeMethod(MethodInfo method, Type[] genericArgs, object[] valueArgs)
        {
            if (!method.DeclaringType.IsAssignableFrom(_target.GetType()))
            {
                method = _target.GetType().GetMethods().SingleOrDefault(m => SignatureEqual(m, method));
                if (method == null)
                {
                    throw new NotImplementedException();
                }
            }

            if (method.IsGenericMethodDefinition)
            {
                method = method.MakeGenericMethod(genericArgs);
            }

            return method.Invoke(_target, valueArgs);
        }

        private bool SignatureEqual(MethodInfo method1, MethodInfo method2)
        {
            if (method1.Name != method2.Name)
            {
                return false;
            }

            if (method1.ReturnType != method2.ReturnType)
            {
                return false;
            }

            if (method1.IsSpecialName != method2.IsSpecialName)
            {
                return false;
            }

            var tArgs1 = method1.GetGenericArguments();
            var tArgs2 = method2.GetGenericArguments();

            if (tArgs1.Length != tArgs2.Length)
            {
                return false;
            }

            var vArgs1 = method1.GetParameters();
            var vArgs2 = method2.GetParameters();

            if (vArgs1.Length != vArgs2.Length)
            {
                return false;
            }

            for (var i = 0; i < vArgs1.Length; i++)
            {
                if (vArgs1[i].ParameterType != vArgs2[i].ParameterType)
                {
                    if (Array.IndexOf(tArgs1, vArgs1[i].ParameterType) != Array.IndexOf(tArgs2, vArgs2[i].ParameterType))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}