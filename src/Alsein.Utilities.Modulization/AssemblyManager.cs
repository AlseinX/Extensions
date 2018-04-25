using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Alsein.Utilities
{
    public class AssemblyManager
    {
        public static Predicate<Assembly> IsToBeLoaded { get; set; } = a => a.FullName.StartsWith(Assembly.GetEntryAssembly().FullName.Split(',')[0].Split('.')[0]);

        private static IEnumerable<Assembly> _allModuleAssemblies;

        public static IEnumerable<Assembly> AllModuleAssemblies
        {
            get
            {
                if (_allModuleAssemblies == null)
                {
                    var entryPath = Assembly.GetEntryAssembly().Location.Replace('\\', '/');
                    entryPath = entryPath.Substring(0, entryPath.LastIndexOf('/'));
                    _allModuleAssemblies =
                    from a in AppDomain.CurrentDomain.GetAssemblies().Union
                    (
                        from ass in
                            from dll in Directory.EnumerateFiles(entryPath, "*.dll", SearchOption.AllDirectories)
                            select Utils.EvaluateOrDefault(() => Assembly.LoadFrom(dll))
                        where ass != null
                        select ass
                    )
                    where IsToBeLoaded(a)
                    orderby a.FullName
                    select a;
                }
                return _allModuleAssemblies.ToArray();
            }
        }
    }
}