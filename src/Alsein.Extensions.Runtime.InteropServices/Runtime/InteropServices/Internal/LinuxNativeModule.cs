using System;
using System.Runtime.InteropServices;

namespace Alsein.Extensions.Runtime.InteropServices.Internal
{
    internal class LinuxNativeModule : NativeModule
    {
        private const string SO_NAME = "libdl.so";

        private const int RTLD_LAZY = 1;

        private readonly IntPtr _module;

        public LinuxNativeModule(string filename)
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

        protected override IntPtr GetGlobalVariable(string variableName) => DlSym(_module, variableName);

        protected override void Dispose() => DlClose(_module);

        [DllImport(SO_NAME, EntryPoint = "dlopen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr DlOpen(string filename, int flags);

        [DllImport(SO_NAME, EntryPoint = "dlsym", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr DlSym(IntPtr module, string procedureName);

        [DllImport(SO_NAME, EntryPoint = "dlclose", CallingConvention = CallingConvention.Cdecl)]
        private static extern int DlClose(IntPtr hModule);

        [DllImport(SO_NAME, EntryPoint = "dlerror", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern string DlError();
    }
}
