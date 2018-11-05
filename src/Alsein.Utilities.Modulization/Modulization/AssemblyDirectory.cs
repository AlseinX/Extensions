using System;
using System.Reflection;

namespace Alsein.Utilities.Modulization
{
    /// <summary>
    /// 
    /// </summary>
    public struct AssemblyDirectory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Func<string, bool> Filter { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Path { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Recursive { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        /// <param name="filter"></param>
        public AssemblyDirectory(string path, bool recursive, Func<string, bool> filter)
        {
            Path = path;
            Recursive = recursive;
            Filter = filter;
        }
    }
}