using Alsein.Utilities.Runtime.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Alsein.Utilities.Runtime.InteropServices.Internal
{
    internal class NativeFunctionDelegates
    {
        private static readonly IDictionary<MethodInfo, Type> _delegates = new ConcurrentDictionary<MethodInfo, Type>();

        public static Type GetDelegateTypeFor(MethodInfo method, NativeModuleAttribute replacementOptions) =>
            _delegates.GetOrCreate(method, () => GenerateDelegateFor(method, replacementOptions));

        private static Type GenerateDelegateFor(MethodInfo method, NativeModuleAttribute replacementOptions)
        {
            var moduleOptions = method.DeclaringType.GetCustomAttribute<NativeModuleAttribute>();
            var funcOptions = method.GetCustomAttribute<NativeFunctionAttribute>();
            var mergedOptions = new NativeFunctionAttribute()
            {
                BestFitMapping = funcOptions?.BestFitMapping ?? replacementOptions?.BestFitMapping ?? moduleOptions?.BestFitMapping ?? true,
                CallingConvention = funcOptions?.CallingConvention ?? replacementOptions?.CallingConvention ?? moduleOptions?.CallingConvention ?? CallingConvention.Cdecl,
                CharSet = funcOptions?.CharSet ?? replacementOptions?.CharSet ?? moduleOptions?.CharSet ?? CharSet.Ansi,
                SetLastError = funcOptions?.SetLastError ?? replacementOptions?.SetLastError ?? moduleOptions?.SetLastError ?? true,
                ThrowOnUnmappableChar = funcOptions?.ThrowOnUnmappableChar ?? replacementOptions?.ThrowOnUnmappableChar ?? moduleOptions?.ThrowOnUnmappableChar ?? false
            };
            var paramsInfo = method.GetParameters();
            var paramTypes = paramsInfo.Select(p => p.ParameterType).ToArray();

            var target = RuntimeAssembly.DefineType(
                $"Alsein.Utilities.Runtime.GeneratedNativeModules.{method.DeclaringType.FullName}.{method.Name}",
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.Sealed |
                TypeAttributes.Public);

            target.SetParent(typeof(MulticastDelegate));

            var ctor = target.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName, CallingConventions.HasThis, new[] { typeof(IntPtr) });
            ctor.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            var beginInvoke = target.DefineMethod("BeginInvoke",
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot |
                MethodAttributes.Virtual,
                typeof(IAsyncResult),
                paramTypes.Concat(new[]
                {
                    typeof(AsyncCallback),
                    typeof(object)
                }).ToArray());

            for (var i = 0; i < paramsInfo.Length; i++)
            {
                beginInvoke.DefineParameter(i + 3, paramsInfo[i].Attributes, paramsInfo[i].Name);
            }

            beginInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            var endInvoke = target.DefineMethod("EndInvoke",
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot |
                MethodAttributes.Virtual,
                method.ReturnType,
                new[] { typeof(IAsyncResult) });

            endInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            var invoke = target.DefineMethod("Invoke",
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot |
                MethodAttributes.Virtual,
                CallingConventions.Standard,
                method.ReturnType,
                paramTypes);

            for (var i = 0; i < paramsInfo.Length; i++)
            {
                invoke.DefineParameter(i + 1, paramsInfo[i].Attributes, paramsInfo[i].Name);
            }

            invoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

            var unmanagedFunctionPointerAttribute = typeof(UnmanagedFunctionPointerAttribute);
            target.SetCustomAttribute(new CustomAttributeBuilder(
                unmanagedFunctionPointerAttribute.GetConstructor(new[] { typeof(CallingConvention) }),
                new object[] { mergedOptions.CallingConvention },
                new[]
                {
                    "BestFitMapping",
                    "CharSet",
                    "SetLastError",
                    "ThrowOnUnmappableChar"
                }.Select(name => unmanagedFunctionPointerAttribute.GetField(name)).ToArray(),
                new object[]
                {
                    mergedOptions.BestFitMapping,
                    mergedOptions.CharSet,
                    mergedOptions.SetLastError,
                    mergedOptions.ThrowOnUnmappableChar
                }));

            return target.CreateTypeInfo();
        }
    }
}
