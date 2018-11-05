using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Alsein.Utilities.Extensions;
using Microsoft.Extensions.Options;

namespace Alsein.Utilities.Modulization.Internal
{

    /// <summary>
    /// 
    /// </summary>
    internal class AssemblyManager : IAssemblyManager
    {
        private readonly AssemblyManagerOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public AssemblyManager(IOptions<AssemblyManagerOptions> options)
        {
            _options = options.Value;
            Features = new FeaturesQuerier(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Assembly> ProjectAssemblies => AppDomain.CurrentDomain.GetAssemblies().Where(_options.ProjectAssemblyFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, IEnumerable<Type>> Features { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void LoadExternalAssemblies() =>
            _options.ExternalDirectories
                .SelectMany(dir =>
                    Directory
                        .EnumerateFiles(dir.Path, "*.dll", dir.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                        .Where(dir.Filter)
                )
                .Where(IsManagedAssembly)
                .ForAll(Assembly.LoadFrom, MassiveExecutionFlags.IgnoreExceptions);

        /// <summary>
        /// <see href="https://stackoverflow.com/a/15608028/8675026">Reference: https://stackoverflow.com/a/15608028/8675026</see>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsManagedAssembly(string fileName)
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

        private class FeaturesQuerier : IReadOnlyDictionary<string, IEnumerable<Type>>
        {
            private AssemblyManager _target;

            public FeaturesQuerier(AssemblyManager target) => _target = target;

            private IEnumerable<Type> AllTypes => _target.ProjectAssemblies.SelectMany(asm => asm.ExportedTypes);

            public IEnumerable<Type> this[string key] => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();

            public IEnumerable<string> Keys => _target._options.FeatureFilters.Keys;

            public IEnumerable<IEnumerable<Type>> Values => this.Select(feature => feature.Value);

            public int Count => _target._options.FeatureFilters.Count;

            public bool ContainsKey(string key) => _target._options.FeatureFilters.ContainsKey(key);

            public IEnumerator<KeyValuePair<string, IEnumerable<Type>>> GetEnumerator() =>
                AllTypes
                    .ToArray()
                    .To(types =>
                        _target._options.FeatureFilters.Select(feature =>
                            new KeyValuePair<string, IEnumerable<Type>>(
                                feature.Key,
                                types.Where(feature.Value)
                            )
                        )
                    )
                    .GetEnumerator();

            public bool TryGetValue(string key, out IEnumerable<Type> value)
            {
                value = _target._options.FeatureFilters.TryGetValue(key, out var filter) ? AllTypes.Where(filter) : default;
                return value != default;
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}