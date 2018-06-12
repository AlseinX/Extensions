using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Alsein.Utilities
{

    /// <summary>
    /// 
    /// </summary>
    public class AssemblyLoader
    {
        /// <summary>
        /// 
        /// </summary>
        public static FunctionGenerator<Assembly, Func<Assembly, bool>> IsSharingRootName { get; } = new FunctionGenerator<Assembly, Func<Assembly, bool>>(
            entry => assembly => assembly.FullName.StartsWith(entry.FullName.Split(',')[0].Split('.')[0]),
            Assembly.GetEntryAssembly()
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> LoadAssemblies(Assembly entry = null, bool recursive = false)
        {
            Directory.EnumerateFiles(Path.GetDirectoryName(
                (entry ?? Assembly.GetEntryAssembly()).Location),
                "*.dll",
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly
                ).Where(IsManagedAssembly)
                .ForAll(Assembly.LoadFrom);
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// <see href="https://stackoverflow.com/a/15608028/8675026">Reference: https://stackoverflow.com/a/15608028/8675026</see>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsManagedAssembly(string fileName)
        {
            using (Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                if (fileStream.Length < 64)
                {
                    return false;
                }

                fileStream.Position = 0x3C;
                uint peHeaderPointer = binaryReader.ReadUInt32();
                if (peHeaderPointer == 0)
                {
                    peHeaderPointer = 0x80;
                }

                if (peHeaderPointer > fileStream.Length - 256)
                {
                    return false;
                }

                fileStream.Position = peHeaderPointer;
                uint peHeaderSignature = binaryReader.ReadUInt32();
                if (peHeaderSignature != 0x00004550)
                {
                    return false;
                }

                fileStream.Position += 20;

                const ushort PE32 = 0x10b;
                const ushort PE32Plus = 0x20b;

                var peFormat = binaryReader.ReadUInt16();
                if (peFormat != PE32 && peFormat != PE32Plus)
                {
                    return false;
                }

                ushort dataDictionaryStart = (ushort)(peHeaderPointer + (peFormat == PE32 ? 232 : 248));
                fileStream.Position = dataDictionaryStart;

                uint cliHeaderRva = binaryReader.ReadUInt32();
                if (cliHeaderRva == 0)
                {
                    return false;
                }

                return true;
            }
        }
    }
}