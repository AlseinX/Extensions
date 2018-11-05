using System;
using System.Collections.Generic;

namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public struct DisplayItem<TKey> :
        IEquatable<DisplayItem<TKey>>,
        IEquatable<(TKey, string)>,
        IEquatable<KeyValuePair<TKey, string>>,
        IEquatable<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TKey Key { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Value { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public DisplayItem(TKey key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public DisplayItem(TKey key)
        {
            Key = key;
            switch (key)
            {
                case string value:
                    Value = value;
                    break;

                case Enum _:
                    Value = Enum.GetName(key.GetType(), key);
                    break;

                default:
                    Value = key.ToString();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => obj is DisplayItem<TKey> other ? Equals(other) : false;

        /// <summary>
        /// 
        /// </summary>
        public static implicit operator (TKey, string) (DisplayItem<TKey> source) => (source.Key, source.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator DisplayItem<TKey>((TKey key, string value) source) => new DisplayItem<TKey>(source.key, source.value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static implicit operator KeyValuePair<TKey, string>(DisplayItem<TKey> source) => new KeyValuePair<TKey, string>(source.Key, source.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator DisplayItem<TKey>(KeyValuePair<TKey, string> source) => new DisplayItem<TKey>(source.Key, source.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator string(DisplayItem<TKey> source) => source.Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static implicit operator DisplayItem<TKey>(TKey key) => new DisplayItem<TKey>(key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public static bool operator ==(DisplayItem<TKey> item1, DisplayItem<TKey> item2) => item1.Equals(item2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public static bool operator !=(DisplayItem<TKey> item1, DisplayItem<TKey> item2) => !(item1 == item2);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Deconstruct(out TKey key, out string value)
        {
            key = Key;
            value = Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DisplayItem<TKey> other) =>
            EqualityComparer<TKey>.Default.Equals(Key, other.Key) &&
            Value == other.Value;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Equals((TKey, string) other) => this == other;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(KeyValuePair<TKey, string> other) => this == other;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TKey other) => Key.Equals(other);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 206514262;
            hashCode = hashCode * -1521134295 + EqualityComparer<TKey>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
