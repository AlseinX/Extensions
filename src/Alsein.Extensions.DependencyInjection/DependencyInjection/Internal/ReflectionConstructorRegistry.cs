using System.Reflection;
using System.Runtime.Serialization;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class ReflectionConstructorRegistry : RegistryBase
    {
        private ConstructorInfo _constructor;

        private object[] _args;

        public ReflectionConstructorRegistry(ILifetime lifetime, bool injectProperties, ConstructorInfo constructor, object[] args)
        : base(lifetime, injectProperties)
        {
            _constructor = constructor;
            _args = args;
        }

        public override object CreateInstance(IResolver resolver)
        {
            var result = FormatterServices.GetUninitializedObject(_constructor.DeclaringType);

            if (InjectProperties)
            {
                resolver.InjectProperties(result);
            }

            var ps = _constructor.GetParameters();
            var args = new object[ps.Length];

            args.ForArray(_args, ps, (ref object target, ref object provided, ref ParameterInfo info)
                => target = provided ?? resolver.Resolve(info.GetCustomAttribute<InjectedAttribute>()?.Key ?? info.ParameterType));

            _constructor.Invoke(result, args);

            return result;
        }
    }
}