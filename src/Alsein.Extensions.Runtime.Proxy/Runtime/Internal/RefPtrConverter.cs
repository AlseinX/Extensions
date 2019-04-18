using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Alsein.Extensions.Runtime.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class RefPtrConverter
    {
        private static readonly IDictionary<Type, Delegate> _refToPtr;

        private static readonly IDictionary<Type, Delegate> _ptrToRef;

        static RefPtrConverter()
        {
            _refToPtr = new Dictionary<Type, Delegate>();
            _ptrToRef = new Dictionary<Type, Delegate>();
        }

        private static Delegate DefineRefToPtr<T>()
        {
            var refToPtr = new DynamicMethod("RefToPtr", typeof(IntPtr), new[] { typeof(T).MakeByRefType() });
            var il = refToPtr.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ret);
            return refToPtr.CreateDelegate(typeof(DRefToPtr<T>));
        }

        private static Delegate DefinePtrToRef<T>()
        {
            var ptrToRef = new DynamicMethod("PtrToRef", typeof(T).MakeByRefType(), new[] { typeof(IntPtr) });
            var il = ptrToRef.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ret);
            return ptrToRef.CreateDelegate(typeof(DPtrToRef<T>));
        }

        private delegate IntPtr DRefToPtr<T>(ref T x);

        private delegate ref T DPtrToRef<T>(IntPtr x);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IntPtr RefToPtr<T>(ref T obj) =>
            ((DRefToPtr<T>)_refToPtr.GetOrCreate(typeof(T), DefineRefToPtr<T>))(ref obj);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static ref T PtrToRef<T>(IntPtr ptr) =>
            ref ((DPtrToRef<T>)_ptrToRef.GetOrCreate(typeof(T), DefinePtrToRef<T>))(ptr);
    }
}
