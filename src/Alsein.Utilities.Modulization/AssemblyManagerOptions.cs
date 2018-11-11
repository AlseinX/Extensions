using System;
using System.Collections.Generic;
using System.IO;
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
        public Assembly EntryAssembly { get; set; }

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
            EntryAssembly = Assembly.GetEntryAssembly();
            ExternalDirectories = new List<AssemblyDirectory>();
            ProjectAssemblyFilter = asm => asm.IsSharingRootNamespace(EntryAssembly);
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
            ExternalDirectories.Add(new AssemblyDirectory(Path.GetDirectoryName(EntryAssembly.Location), false, path =>
                Path.GetFileNameWithoutExtension(path).StartsWith(EntryAssembly.FullName.Split(',')[0].Split('.')[0]))
            );
        }
    }
}