using System;
using System.Runtime.InteropServices;

namespace Alsein.Utilities.Runtime.InteropServices.Internal
{
    internal class OSXNativeModule : NativeModule
    {
        private const string DYLIB_NAME = "libdl.dylib";

        private const int RTLD_LAZY = 1;

        private readonly IntPtr _module;

        public OSXNativeModule(string filename)
        {
            _module = DlOpen(filename, RTLD_LAZY);
            if (_module == IntPtr.Zero)
            {
                throw new DllNotFoundException(DlError());
            }
        }

        protected override Delegate GetFunction(string functionName, Type delegateType)
        {
            var proc = DlSym(_module, functionName);
            return Marshal.GetDelegateForFunctionPointer(proc, delegateType);
        }

        protected override void Dispose() => DlClose(_module);

        [DllImport(DYLIB_NAME, EntryPoint = "dlopen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr DlOpen(string filename, int flags);

        [DllImport(DYLIB_NAME, EntryPoint = "dlsym", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr DlSym(IntPtr module, string procedureName);

        [DllImport(DYLIB_NAME, EntryPoint = "dlclose", CallingConvention = CallingConvention.Cdecl)]
        private static extern int DlClose(IntPtr hModule);

        [DllImport(DYLIB_NAME, EntryPoint = "dlerror", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern string DlError();
    }
}
