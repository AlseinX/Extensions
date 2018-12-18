using Alsein.Utilities.Runtime.InteropServices.Internal;
using System;
using System.Runtime.InteropServices;

namespace Alsein.Utilities.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    public class NativeModuleFactory : INativeModuleFactory
    {
        internal static INativeModule LoadAssembly(string filename)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new LinuxNativeModule(filename);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsNativeModule(filename);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new OSXNativeModule(filename);
            }
            throw new PlatformNotSupportedException();
        }

        INativeModule INativeModuleFactory.LoadModule(string filename) => LoadAssembly(filename);
    }
}
