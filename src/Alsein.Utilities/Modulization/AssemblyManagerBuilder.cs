using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyManagerBuilder : IAssemblyManagerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 
        /// </summary>
        public AssemblyManagerBuilder(IServiceCollection services = null)
        {
            Services = services ?? new ServiceCollection();
            Services.AddSingleton<IAssemblyManager, AssemblyManager>();
            var entry = Assembly.GetEntryAssembly();
            Services.Configure<AssemblyManagerOptions>(o =>
                o.ExternalDirectories.Add(new AssemblyDirectory(Path.GetDirectoryName(entry.Location), false, path =>
                    Path.GetFileNameWithoutExtension(path).StartsWith(entry.FullName.Split(',')[0].Split('.')[0]))
                )
            );
        }
    }
}