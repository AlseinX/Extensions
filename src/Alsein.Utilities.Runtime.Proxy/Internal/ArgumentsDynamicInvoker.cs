using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Alsein.Utilities.Runtime.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ArgumentsDynamicInvoker
    {
        private static readonly IDictionary<Type, Invoker> _invokers = new Dictionary<Type, Invoker>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IArguments Invoke(IArguments arguments, Delegate target) =>
            _invokers.GetOrCreate(target.GetType(), () => DefineInvoker(target.GetType()))(target, arguments);

        private static readonly MethodInfo _at = typeof(IArguments).GetMethod(nameof(IArguments.At));
        private static readonly ConstructorInfo _newArgumentsBuilder = typeof(ArgumentsBuilder).GetConstructor(new[] { typeof(int) });
        private static readonly MethodInfo _setByVal = typeof(ArgumentsBuilder).GetMethod(nameof(ArgumentsBuilder.SetByVal), m => m.ContainsGenericParameters);
        private static readonly MethodInfo _setByRef = typeof(ArgumentsBuilder).GetMethod(nameof(ArgumentsBuilder.SetByRef));
        private static readonly MethodInfo _setVoid = typeof(ArgumentsBuilder).GetMethod(nameof(ArgumentsBuilder.SetVoid));
        private static readonly MethodInfo _build = typeof(ArgumentsBuilder).GetMethod(nameof(ArgumentsBuilder.Build));

        private static Invoker DefineInvoker(Type delegateType)
        {
            var invoke = delegateType.GetMethod("Invoke");
            var paras = invoke.GetParameters();
            var invoker = new DynamicMethod("${delegateType}Invoker",
                typeof(IArguments), new[] { typeof(Delegate), typeof(IArguments) });
            var il = invoker.GetILGenerator();


            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Newobj, _newArgumentsBuilder);

            il.Emit(OpCodes.Ldc_I4_0);

            il.Emit(OpCodes.Ldarg_0);

            for (var i = 0; i < paras.Length; i++)
            {
                var paramType = paras[i].ParameterType;
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                if (paramType.IsByRef)
                {
                    il.Emit(OpCodes.Callvirt, _at.MakeGenericMethod(paramType.GetElementType()));
                }
                else
                {
                    il.Emit(OpCodes.Callvirt, _at.MakeGenericMethod(paramType));
                    il.Emit(OpCodes.Ldobj, paramType);
                }
            }

            il.Emit(OpCodes.Callvirt, invoke);

            if (invoke.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Call, _setVoid);
            }
            else if (invoke.ReturnType.IsByRef)
            {
                il.Emit(OpCodes.Call, _setByRef.MakeGenericMethod(invoke.ReturnType.GetElementType()));
            }
            else
            {
                il.Emit(OpCodes.Call, _setByVal.MakeGenericMethod(invoke.ReturnType));
            }

            il.Emit(OpCodes.Call, _build);
            il.Emit(OpCodes.Ret);

            return (Invoker)invoker.CreateDelegate(typeof(Invoker));
        }

        private delegate IArguments Invoker(Delegate target, IArguments args);
    }
}
