using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Alsein.Extensions.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class List<T> : IRefList<T>
    {
        private const int _staticCapacity = 8;

        private const int _log2StaticCapacity = 3;

        private static readonly T[][] _emptyData = new T[0][];

        private IList<T[]> _data = _emptyData;

        private int _level = 0;

        private int _size = 0;

        /// <summary>
        /// 
        /// </summary>
        public List() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public List(int size) { }

        private void EnsureCapacity(int cap)
        {
            var level = (int)Ceiling(Log(cap + _staticCapacity, 2)) - _log2StaticCapacity;

            if (level <= _level)
            {
                return;
            }

            if (level > _staticCapacity)
            {
                if (_data is List<T[]> data)
                {
                    data.Count = level;
                }
                else
                {
                    _data = new List<T[]>(level);
                }
            }
            else
            {
                if (_level == 0)
                {
                    _data = new T[_staticCapacity][];
                }
            }

            for (var i = _level; i < level; i++)
            {
                _data[i] = new T[(int)Pow(2, i + _log2StaticCapacity)];
            }

            _level = level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public ref T this[int index]
        {
            get
            {
                var level = (int)Floor(Log(index + 1 + _staticCapacity, 2)) - _log2StaticCapacity - 1;
                return ref _data[level][index - ((int)Pow(2, level + _log2StaticCapacity)) + _staticCapacity];
            }
        }


        T IList<T>.this[int index] { get => this[index]; set => this[index] = value; }

        T IReadOnlyList<T>.this[int index] { get => this[index]; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int Count
        {
            get => _size;
            set
            {
                EnsureCapacity(value);
                _size = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            EnsureCapacity(_size + 1);
            this[_size] = item;
            _size++;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _size = 0;
            _level = 0;
            _data = _emptyData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            foreach (var elem in this)
            {
                if (elem.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex) => CopyTo(array, arrayIndex, 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="count"></param>
        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            var i = arrayIndex;
            var ending = arrayIndex + count;

            foreach (var elem in this)
            {
                if (i >= ending)
                {
                    break;
                }
                array[i] = elem;
                i++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public struct Enumerator : IRefEnumerator<T>
        {
            private List<T> _this;

            private int _i;

            private int _j;

            private int _index;

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public int Index
            {
                get => _index;
                set
                {
                    var level = (int)Floor(Log(value + 1 + _staticCapacity, 2)) - _log2StaticCapacity - 1;
                    _i = level;
                    _j = value - ((int)Pow(2, level + _log2StaticCapacity)) + _staticCapacity;
                    _nextEnding = (int)Pow(2, _i + _log2StaticCapacity + 1);
                }
            }

            private int _nextEnding;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="this"></param>
            public Enumerator(List<T> @this)
            {
                _this = @this;
                _i = 0;
                _j = 0;
                _index = 0;
                _nextEnding = _staticCapacity;
            }

            /// <summary>
            /// 
            /// </summary>
            public ref T Current => ref _this._data[_i][_j];

            T IEnumerator<T>.Current => _this._data[_i][_j];

            object IEnumerator.Current => _this._data[_i][_j];

            /// <summary>
            /// 
            /// </summary>
            public void Dispose() { }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (++_index >= _this._size)
                {
                    return false;
                }
                if (++_i >= _nextEnding)
                {
                    _nextEnding *= 2;
                    _i = 0;
                    _j++;
                }
                return true;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                _i = 0;
                _j = 0;
                _index = 0;
                _nextEnding = _staticCapacity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRefEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            var enumerator = new Enumerator(this);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(item))
                {
                    return enumerator._index;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            EnsureCapacity(_size + 1);
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

    }
}