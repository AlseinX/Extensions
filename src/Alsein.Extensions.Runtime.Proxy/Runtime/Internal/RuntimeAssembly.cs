using System.Reflection;
using System.Reflection.Emit;

namespace Alsein.Extensions.Runtime.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeAssembly
    {
        private static AssemblyBuilder _assemblyBuilder;

        private static ModuleBuilder _moduleBuilder;

        private static AssemblyBuilder AssemblyBuilder => _assemblyBuilder ?? (
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("Alsein.Extensions.Runtime.GeneratedAssembly"),
                AssemblyBuilderAccess.Run
            )
        );

        private static ModuleBuilder ModuleBuilder => _moduleBuilder ?? (
            _moduleBuilder = AssemblyBuilder.DefineDynamicModule("Alsein.Extensions.Runtime.GeneratedAssembly")
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TypeBuilder DefineType(string name) => ModuleBuilder.DefineType(name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="typeAttributes"></param>
        /// <returns></returns>
        public static TypeBuilder DefineType(string name, TypeAttributes typeAttributes) => ModuleBuilder.DefineType(name, typeAttributes);
    }
}