using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextedBase<TClass>
        where TClass : ContextedBase<TClass>
    {
        private static object _locker = new object();

        private static IServiceProvider _contextProvider = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextProvider"></param>
        public static IDisposableWithStatus WithContext(IServiceProvider contextProvider)
        {
            if (contextProvider is null)
            {
                throw new ArgumentNullException(nameof(contextProvider));
            }
            var locker = new DisposibleLocker(_locker);
            locker.Disposing += l =>
            {
                _contextProvider = null;
            };
            _contextProvider = contextProvider;
            return locker;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsContextProviderSet => Monitor.IsEntered(_locker);

        /// <summary>
        /// 
        /// </summary>
        public ContextedBase()
        {
            if (_contextProvider is null)
            {
                throw new ContextNotPresetException();
            }

            var contextProvider = _contextProvider;

            var reflectedTypes = new List<Type>();
            {
                var curt = GetType();
                var objType = typeof(object);
                var target = typeof(ContextedBase<>).MakeGenericType(curt);

                reflectedTypes.Add(curt);
                while ((curt = curt.BaseType) != target)
                {
                    if (curt == objType)
                    {
                        throw new InvalidOperationException();
                    }
                    reflectedTypes.Add(curt);
                }
            }

            var properties = reflectedTypes.SelectMany(t => t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            foreach (var p in properties)
            {
                if ((!p.PropertyType.IsValueType) && p.CanRead && p.CanWrite && p.GetValue(this) == null)
                {
                    if (contextProvider.GetService(p.PropertyType) is object o)
                    {
                        p.SetValue(this, o);
                    }
                }
            }
        }
    }
}
