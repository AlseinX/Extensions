using System.Reflection;
using System.Reflection.Emit;

namespace Alsein.Utilities.Runtime.Internal
{
    internal class RuntimeAssembly
    {
        private static AssemblyBuilder _assemblyBuilder;

        private static ModuleBuilder _moduleBuilder;

        public static AssemblyBuilder AssemblyBuilder => _assemblyBuilder ?? (
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("Alsein.Utilities.Runtime.GeneratedAssembly"),
                AssemblyBuilderAccess.Run
            )
        );

        public static ModuleBuilder ModuleBuilder => _moduleBuilder ?? (
            _moduleBuilder = AssemblyBuilder.DefineDynamicModule("Alsein.Utilities.Runtime.GeneratedAssembly")
        );
    }
}