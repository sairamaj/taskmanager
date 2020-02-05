using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Core.Extensions
{
    /// <summary>
    /// Linq extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Flattens the hierarchical list.
        /// </summary>
        /// <typeparam name="T">
        /// Type name.
        /// </typeparam>
        /// <typeparam name="TR">
        /// Type name2.
        /// </typeparam>
        /// <param name="source">
        /// Source list.
        /// </param>
        /// <param name="recursion">
        /// Recursive action.
        /// </param>
        /// <returns>
        /// Flatten list.
        /// source: https://stackoverflow.com/questions/19237868/get-all-children-to-one-list-recursive-c-sharp/21054096#21054096.
        /// </returns>
        public static IEnumerable<T> Flatten<T, TR>(this IEnumerable<T> source, Func<T, TR> recursion)
            where TR : IEnumerable<T>
        {
            if (source == null)
            {
                return new List<T>();
            }

            source = source.ToList();
            var flattened = source.ToList();
            var children = source.Select(recursion);

            foreach (var child in children)
            {
                flattened.AddRange(child.Flatten(recursion));
            }

            return flattened;
        }
    }
}
