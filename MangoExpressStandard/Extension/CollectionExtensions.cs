using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MangoExpressStandard.Extension
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CollectionExtensions
    {
        /// <summary>
        /// Checks whether or not collection is null or empty. 
        /// Assumes collection can be safely enumerated multiple times.
        /// </summary>
        /// <returns><c>true</c>, if null or empty was ised, <c>false</c> otherwise.</returns>
        /// <param name="this">This.</param>
        public static bool IsNullOrEmpty(this IEnumerable @this)
        {
            return @this == null || !@this.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// Generates a HashCode for the contents for the list.
        /// Order of items does not matter.
        /// </summary>
        /// <returns>The contents hash code.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static int GetContentsHashCode<T>(IList<T> list)
        {
            if (list == null)
                return 0;

            int num = 0;
            for (int index = 0; index < list.Count; ++index)
            {
                if ((object)list[index] != null)
                    num += list[index].GetHashCode();
            }

            return num;
        }
    }
}
