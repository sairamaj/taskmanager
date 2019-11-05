using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Core
{
    // https://github.com/jehugaleahsa/ComparerExtensions/blob/master/ComparerExtensions/KeyEqualityComparer.cs

    /// <summary>
    /// Key equality comparer.
    /// </summary>
    /// <typeparam name="T">
    /// Type name.
    /// </typeparam>
    public class KeyEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Comparer function.
        /// </summary>
        private readonly Func<T, T, bool> _comparer;

        /// <summary>
        /// Key extractor.
        /// </summary>
        private readonly Func<T, object> _keyExtractor;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="keyExtractor">
        /// Key extractor func.
        /// </param>
        public KeyEqualityComparer(Func<T, object> keyExtractor)
            : this(keyExtractor, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// Comparer func.
        /// </param>
        public KeyEqualityComparer(Func<T, T, bool> comparer)
            : this(null, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="keyExtractor">
        /// Key extractor func.
        /// </param>
        /// <param name="comparer">
        /// Comparer func.
        /// </param>
        public KeyEqualityComparer(Func<T, object> keyExtractor, Func<T, T, bool> comparer)
        {
            this._keyExtractor = keyExtractor;
            this._comparer = comparer;
        }

        /// <summary>
        /// Equality function.
        /// </summary>
        /// <param name="x">
        /// A first instance.
        /// </param>
        /// <param name="y">
        /// A second instance.
        /// </param>
        /// <returns>
        /// true if these two instances are equal.
        /// </returns>
        public bool Equals(T x, T y)
        {
            if (this._comparer != null)
            {
                return this._comparer(x, y);
            }

            var valX = this._keyExtractor(x);

            // The special case where we pass a list of keys
            if (valX is IEnumerable<object>)
            {
                return ((IEnumerable<object>)valX).SequenceEqual((IEnumerable<object>)this._keyExtractor(y));
            }

            return valX.Equals(this._keyExtractor(y));
        }

        /// <summary>
        /// Gets hash code of object instance.
        /// </summary>
        /// <param name="obj">
        /// Instance object.
        /// </param>
        /// <returns>
        /// Hash code.
        /// </returns>
        public int GetHashCode(T obj)
        {
            if (this._keyExtractor == null)
            {
                return obj.ToString().ToLower().GetHashCode();
            }

            var val = this._keyExtractor(obj);

            // The special case where we pass a list of keys
            if (val is IEnumerable<object> objects)
            {
                return (int)objects.Aggregate((x, y) => x.GetHashCode() ^ y.GetHashCode());
            }

            return val.GetHashCode();
        }
    }
}