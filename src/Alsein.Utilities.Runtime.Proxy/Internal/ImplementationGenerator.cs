using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Alsein.Utilities.Runtime.Internal
{
    internal class ImplementationGenerator
    {
        private static IDictionary<Type, IDictionary<Type, Type>> Implements { get; } = new Dictionary<Type, IDictionary<Type, Type>>();

        internal static Type GetImplemention(Type target, Type parent)
        {
            if (!target.IsInterface)
            {
                throw new InvalidOperationException("The target type must be an interface.");
            }

            if (typeof(IReflectionInvoker).IsAssignableFrom(target))
            {
                throw new InvalidOperationException("The target type must not implement IReflectionInvoker.");
            }

            lock (Implements)
            {
                if (!Implements.TryGetValue(target, out var list))
                {
                    Implements.Add(target, list = new Dictionary<Type, Type>());
                }


                if (!list.TryGetValue(parent, out var result))
                {
                    list.Add(parent, result = BuildType(target, parent));
                }

                return result;
            }
        }

        private static MethodInfo[] GetAllMethods(Type type) =>
            type.GetMethods()
            .Union(type.GetInterfaces().SelectMany(GetAllMethods))
            .ToArray();

        private static TypeInfo BuildType(Type target, Type parent)
        {
            var isInvoker = parent.GetInterfaces().Contains(typeof(IReflectionInvoker));
            var invoke = typeof(IReflectionInvoker).GetMethod(nameof(IReflectionInvoker.Invoke));
            var targetDef = target;
            var parentDef = parent;
            var getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
            var getMethodFromHandle = typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle), new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) });
            var makeGenericMethod = typeof(Type).GetMethod(nameof(Type.MakeGenericType));
            var newNotImplementedException = typeof(NotImplementedException).GetConstructor(new Type[] { });

            var typ = RuntimeAssembly.DefineType($"Alsein.Utilities.GeneratedProxies.{parent.Name}For{target.Name}");
            if (target.IsGenericTypeDefinition)
            {
                var pGenParams = target.GetGenericArguments();
                var genParams = typ.DefineGenericParameters(pGenParams.Select(a => a.Name).ToArray());

                for (var i = 0; i < genParams.Length; i++)
                {
                    genParams[i].SetGenericParameterAttributes(pGenParams[i].GenericParameterAttributes);
                    var intfs = new List<Type>();
                    var baseType = default(Type);
                    var constraints = pGenParams[i].GetGenericParameterConstraints();
                    foreach (var con in constraints)
                    {
                        if (con.IsInterface)
                        {
                            intfs.Add(con);
                        }
                        else
                        {
                            baseType = con;
                        }
                    }
                    genParams[i].SetBaseTypeConstraint(baseType);
                }

                target = target.MakeGenericType(genParams);
                if (parent.IsGenericTypeDefinition)
                {
                    parent = parent.MakeGenericType(genParams);
                }
            }

            typ.SetParent(parent);
            typ.AddInterfaceImplementation(target);

            var constructors = parentDef.GetConstructors();

            foreach (var pCtor in constructors)
            {
                var paras = pCtor.GetParameters();
                var ctor = typ.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, paras.Select(p => p.ParameterType).ToArray());
                var i = 1;
                foreach (var para in paras)
                {
                    ctor.DefineParameter(i, para.Attributes, para.Name);
                    i++;
                }
                var il = ctor.GetILGenerator();
                // base(args...)
                il.Emit(OpCodes.Ldarg_0);
                i = 1;
                foreach (var para in paras)
                {
                    il.Emit(OpCodes.Ldarg, i);
                    i++;
                }
                il.Emit(OpCodes.Call, pCtor);
                il.Emit(OpCodes.Ret);
            }

            var properties = targetDef.GetProperties()
                .Select(propertyInfo => typ.DefineProperty(
                    propertyInfo.Name,
                    PropertyAttributes.None,
                    CallingConventions.Standard,
                    propertyInfo.PropertyType,
                    propertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray())
                )
                .ToArray();

            var methods = GetAllMethods(targetDef);
            for (var i = 0; i < methods.Length; i++)
            {
                var methodInfo = methods[i];
                var match = parentDef.GetMethods().SingleOrDefault(m => SignatureEqual(m, methodInfo));
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

                if (match != null)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    for (var j = 0; j < vArgTypes.Length; j++)
                    {
                        il.Emit(OpCodes.Ldarg, j + 1);
                    }
                    il.Emit(OpCodes.Call, match);
                    il.Emit(OpCodes.Ret);
                }
                else if (!isInvoker)
                {
                    il.Emit(OpCodes.Newobj, newNotImplementedException);
                    il.Emit(OpCodes.Throw);
                }
                else
                {

                    var tArgs = default(LocalBuilder);

                    if (tArgTypes.Length > 0)
                    {
                        // Type[] tArgs;
                        tArgs = il.DeclareLocal(typeof(Type[]));

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
                    }

                    // object[] vArgs;
                    var vArgs = il.DeclareLocal(typeof(object[]));


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

                    // this.Invoke(i, ilVArgs);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldtoken, methodInfo);
                    il.Emit(OpCodes.Ldtoken, targetDef);
                    il.Emit(OpCodes.Call, getMethodFromHandle);
                    if (tArgTypes.Length > 0)
                    {
                        il.Emit(OpCodes.Ldloc, tArgs);
                        il.Emit(OpCodes.Call, makeGenericMethod);
                    }
                    il.Emit(OpCodes.Ldloc, vArgs);
                    il.Emit(OpCodes.Callvirt, invoke);

                    // return ?;
                    if (methodInfo.ReturnType == typeof(void))
                    {
                        il.Emit(OpCodes.Pop);
                    }
                    else if (methodInfo.ReturnType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);
                    }
                    il.Emit(OpCodes.Ret);
                }

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

        private static bool TypeEqual(Type type1, Type type2) => type1.IsGenericParameter && type2.IsGenericParameter ? type1.Name == type2.Name : type1 == type2;

        private static bool SignatureEqual(MethodInfo method1, MethodInfo method2)
        {
            if (method1.Name != method2.Name)
            {
                return false;
            }

            if (!TypeEqual(method1.ReturnType, method2.ReturnType))
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
                if (!TypeEqual(vArgs1[i].ParameterType, vArgs2[i].ParameterType))
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