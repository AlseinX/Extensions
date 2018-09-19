using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyManager
    {
        /// <summary>
        /// 
        /// </summary>
        void LoadExternalAssemblies();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Assembly> ProjectAssemblies { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, IEnumerable<Type>> Features { get; }
    }
}