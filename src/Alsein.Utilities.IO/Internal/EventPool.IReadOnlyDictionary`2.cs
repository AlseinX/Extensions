using System;
using System.Collections;
using System.Collections.Generic;

namespace Alsein.Utilities.IO.Internal
{
    internal partial class EventPool : IReadOnlyDictionary<object, IEvents<object>>
    {
        public IEvents<object> this[object key] => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList)[key];

        public IEnumerable<object> Keys => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList).Keys;

        public IEnumerable<IEvents<object>> Values => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList).Values;

        public int Count => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList).Count;

        public bool ContainsKey(object key) => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList).ContainsKey(key);

        public IEnumerator<KeyValuePair<object, IEvents<object>>> GetEnumerator() => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList).GetEnumerator();

        public bool TryGetValue(object key, out IEvents<object> value) => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList).TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyDictionary<object, IEvents<object>>)_eventsList).GetEnumerator();
    }
}