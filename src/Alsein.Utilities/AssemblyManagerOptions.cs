using System.Collections.Generic;

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
        public IList<AssemblyDirectory> Directories { get; }

        /// <summary>
        /// 
        /// </summary>
        public AssemblyManagerOptions()
        {
            Directories = new List<AssemblyDirectory>();
        }
    }
}