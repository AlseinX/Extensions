using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Alsein.Extensions.Runtime.Internal
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

            if (typeof(IProxyInvoker).IsAssignableFrom(target))
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
            var isInvoker = parent.GetInterfaces().Contains(typeof(IProxyInvoker));
            var invoke = typeof(IProxyInvoker).GetMethod(nameof(IProxyInvoker.Invoke));
            var targetDef = target;
            var parentDef = parent;

            var newArgumentsBuilder = typeof(ArgumentsBuilder).GetConstructor(new[] { typeof(int) });
            var setByVal = typeof(ArgumentsBuilder).GetMethod(nameof(ArgumentsBuilder.SetByVal), m => m.ContainsGenericParameters);
            var setByRef = typeof(ArgumentsBuilder).GetMethod(nameof(ArgumentsBuilder.SetByRef));
            var build = typeof(ArgumentsBuilder).GetMethod(nameof(ArgumentsBuilder.Build));
            var at = typeof(IArguments).GetMethod(nameof(IArguments.At));
            var getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
            var getMethodFromHandle = typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle), new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) });
            var makeGenericMethod = typeof(Type).GetMethod(nameof(Type.MakeGenericType));
            var newNotImplementedException = typeof(NotImplementedException).GetConstructor(new Type[] { });

            var typ = RuntimeAssembly.DefineType($"Alsein.Extensions.GeneratedProxies.{parent.Name}For{target.Name}");
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
                var paras = methodInfo.GetParameters();
                var vArgTypes = paras.Select(p => p.ParameterType).ToArray();
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

                var k = 1;
                foreach (var para in paras)
                {
                    mtd.DefineParameter(k, para.Attributes, para.Name);
                    k++;
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

                    // ArgumentListBuilder ilVArgs;
                    var vArgs = il.DeclareLocal(typeof(ArgumentsBuilder));


                    // ilVArgs = new ArgumentsBuilder(vArgs.Length);
                    il.Emit(OpCodes.Ldc_I4, vArgTypes.Length);
                    il.Emit(OpCodes.Newobj, newArgumentsBuilder);

                    for (var j = 0; j < vArgTypes.Length; j++)
                    {
                        // .SetByVal/Ref<T>(j, Arguments[j + 1])
                        il.Emit(OpCodes.Ldc_I4, j);
                        il.Emit(OpCodes.Ldarg, j + 1);

                        var setBy = vArgTypes[j].IsByRef ?
                            setByRef.MakeGenericMethod(vArgTypes[j].GetElementType()) :
                            setByVal.MakeGenericMethod(vArgTypes[j]);

                        il.Emit(OpCodes.Call, setBy);
                    }

                    il.Emit(OpCodes.Stloc, vArgs);

                    // this.Invoke(i, ilVArgs.Build());
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
                    il.Emit(OpCodes.Call, build);
                    il.Emit(OpCodes.Callvirt, invoke);

                    // return ?;
                    if (methodInfo.ReturnType == typeof(void))
                    {
                        il.Emit(OpCodes.Pop);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                        if (methodInfo.ReturnType.IsByRef)
                        {
                            il.Emit(OpCodes.Callvirt, at.MakeGenericMethod(methodInfo.ReturnType.GetElementType()));
                        }
                        else
                        {
                            il.Emit(OpCodes.Callvirt, at.MakeGenericMethod(methodInfo.ReturnType));
                            if (methodInfo.ReturnType.IsValueType)
                            {
                                il.Emit(OpCodes.Ldobj, methodInfo.ReturnType);
                            }
                            else
                            {
                                il.Emit(OpCodes.Ldind_Ref);
                            }
                        }
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