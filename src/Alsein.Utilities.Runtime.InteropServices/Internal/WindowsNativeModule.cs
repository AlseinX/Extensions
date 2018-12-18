using System;
using System.Runtime.InteropServices;

namespace Alsein.Utilities.Runtime.InteropServices.Internal
{
    internal class WindowsNativeModule : NativeModule
    {
        private const string DLL_NAME = "kernel32.dll";

        private readonly IntPtr _module;

        public WindowsNativeModule(string filename)
        {
            _module = LoadLibrary(filename);
            if (_module == IntPtr.Zero)
            {
                throw new DllNotFoundException();
            }
        }

        protected override Delegate GetFunction(string functionName, Type delegateType)
        {
            var proc = GetProcAddress(_module, functionName);
            if (proc == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                var tempPtr = IntPtr.Zero;
                var message = default(string);
                FormatMessage(0x1300, ref tempPtr, error, 0, ref message, 255, ref tempPtr);
                throw new NotSupportedException(message);
            }
            return Marshal.GetDelegateForFunctionPointer(proc, delegateType);
        }

        protected override void Dispose() => FreeLibrary(_module);

        [DllImport(DLL_NAME, EntryPoint = "LoadLibraryW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string filename);

        [DllImport(DLL_NAME, EntryPoint = "GetProcAddress", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr module, string procedureName);

        [DllImport(DLL_NAME, EntryPoint = "FreeLibrary", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(DLL_NAME)]
        private static extern int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);
    }
}
