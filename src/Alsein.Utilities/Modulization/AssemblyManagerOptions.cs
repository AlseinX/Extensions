using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyManagerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<AssemblyDirectory> ExternalDirectories { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Func<Assembly, bool> ProjectAssemblyFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, Func<Type, bool>> FeatureFilters { get; }

        /// <summary>
        /// 
        /// </summary>
        public AssemblyManagerOptions()
        {
            ExternalDirectories = new List<AssemblyDirectory>();
            ProjectAssemblyFilter = asm => asm.IsSharingRootNamespace(Assembly.GetEntryAssembly());
            FeatureFilters = new Dictionary<string, Func<Type, bool>>();
            FeatureFilters.Add("Service", type =>
                type.IsInstantiableClass() &&
                !type.IsNonService() &&
                (
                    type.Name.EndsWith("Service") ||
                    type.IsSingletonService() ||
                    type.IsScopedService() ||
                    type.IsTransientService()
                )
            );
        }
    }
}