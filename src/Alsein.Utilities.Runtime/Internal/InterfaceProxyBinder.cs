using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Alsein.Utilities.Runtime.Internal
{
    internal class InterfaceProxyBinder : IProxyBinder
    {

        private static IDictionary<Type, Type> Implements { get; } = new Dictionary<Type, Type>();

        private readonly Type _implementType;

        public Type Target { get; }

        internal InterfaceProxyBinder(Type target)
        {
            Target = target;

            if (!Target.IsInterface)
            {
                throw new InvalidOperationException("The target type must be an interface.");
            }

            if (Target.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException("The target type cannot be an generic definition.");
            }

            if (!Implements.TryGetValue(Target, out _implementType))
            {
                Implements.Add(Target, _implementType = BuildType());
            }
        }

        public object GetProxy(IDynamicInvoker invoker) => Activator.CreateInstance(_implementType, invoker);

        private MethodInfo[] GetAllMethods(Type type) =>
            type.GetMethods()
            .Union(type.GetInterfaces().SelectMany(GetAllMethods))
            .ToArray();

        private TypeInfo BuildType()
        {
            var thisType = this.GetType();
            var dispatchInvocation = typeof(IDynamicInvoker).GetMethod(nameof(IDynamicInvoker.InvokeMethod));
            var getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
            var getMethodFromHandle = typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle), new[] { typeof(RuntimeMethodHandle) });

            var typ = RuntimeAssembly.ModuleBuilder.DefineType($"{Target.Name}Proxy");
            typ.AddInterfaceImplementation(Target);

            var implement = typ.DefineField("_implement", typeof(IDynamicInvoker), FieldAttributes.Private);

            var ctor = typ.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(IDynamicInvoker) });
            {
                var il = ctor.GetILGenerator();
                // this._binder = Arguments[0];
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, implement);
                il.Emit(OpCodes.Ret);
            }

            var properties = Target.GetProperties()
                .Select(propertyInfo => typ.DefineProperty(
                    propertyInfo.Name,
                    PropertyAttributes.None,
                    CallingConventions.Standard,
                    propertyInfo.PropertyType,
                    propertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray())
                )
                .ToArray();

            var methods = GetAllMethods(Target);
            for (var i = 0; i < methods.Length; i++)
            {
                var methodInfo = methods[i];
                var tArgTypes = new GenericTypeParameterBuilder[] { };
                var vArgTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
                var mtd = typ.DefineMethod
                (
                    name: methodInfo.Name,
                    attributes: methodInfo.Attributes & ~MethodAttributes.Abstract,
                    callingConvention: methodInfo.CallingConvention,
                    returnType: methodInfo.ReturnType,
                    parameterTypes: vArgTypes
                );
                if (methodInfo.IsGenericMethodDefinition)
                {
                    tArgTypes = mtd.DefineGenericParameters(methodInfo.GetGenericArguments().Select(t => $"{t.Name}").ToArray());
                }
                var il = mtd.GetILGenerator();

                // Type[] tArgs;
                var tArgs = il.DeclareLocal(typeof(Type[]));
                // object[] vArgs;
                var vArgs = il.DeclareLocal(typeof(object[]));

                // ilTArgs = new Type[tArgs.Length];
                il.Emit(OpCodes.Ldc_I4, tArgTypes.Length);
                il.Emit(OpCodes.Newarr, typeof(Type));
                il.Emit(OpCodes.Stloc, tArgs);

                for (var j = 0; j < tArgTypes.Length; j++)
                {
                    // ilTArgs[j] = typeof(tArgs[j]);
                    il.Emit(OpCodes.Ldloc, tArgs);
                    il.Emit(OpCodes.Ldc_I4, j);
                    il.Emit(OpCodes.Ldtoken, tArgTypes[j]);
                    il.Emit(OpCodes.Call, getTypeFromHandle);
                    il.Emit(OpCodes.Stelem_Ref);
                }

                // ilVArgs = new object[vArgs.Length];
                il.Emit(OpCodes.Ldc_I4, vArgTypes.Length);
                il.Emit(OpCodes.Newarr, typeof(object));
                il.Emit(OpCodes.Stloc, vArgs);

                for (var j = 0; j < vArgTypes.Length; j++)
                {
                    // ilVArgs[j] = (object)Arguments[j];
                    il.Emit(OpCodes.Ldloc, vArgs);
                    il.Emit(OpCodes.Ldc_I4, j);
                    il.Emit(OpCodes.Ldarg, j + 1);
                    if (vArgTypes[j].IsValueType)
                    {
                        il.Emit(OpCodes.Box, vArgTypes[j]);
                    }
                    il.Emit(OpCodes.Stelem_Ref);
                }

                // this._implement.DispatchInvocation(i, ilTArgs, ilVArgs);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, implement);
                il.Emit(OpCodes.Ldtoken, methodInfo);
                il.Emit(OpCodes.Call, getMethodFromHandle);
                il.Emit(OpCodes.Ldloc, tArgs);
                il.Emit(OpCodes.Ldloc, vArgs);
                il.Emit(OpCodes.Callvirt, dispatchInvocation);

                // return ?;
                if (methodInfo.ReturnType == typeof(void))
                {
                    il.Emit(OpCodes.Pop);
                }
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);
                }
                il.Emit(OpCodes.Ret);

                typ.DefineMethodOverride(mtd, methodInfo);

                if (mtd.IsSpecialName && properties.SingleOrDefault(p => p.Name == mtd.Name.Substring(4)) is PropertyBuilder propertyBuilder)
                {
                    if (methodInfo.Name.StartsWith("get_"))
                    {
                        propertyBuilder.SetGetMethod(mtd);
                    }
                    else if (methodInfo.Name.StartsWith("set_"))
                    {
                        propertyBuilder.SetSetMethod(mtd);
                    }
                }
            }

            return typ.CreateTypeInfo();
        }
    }
}