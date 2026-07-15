namespace System.Linq;

internal static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> enumerable)
    {
        [Pure]
        public bool None()
        {
            using var enumerator = enumerable.GetEnumerator();
            return !enumerator.MoveNext();
        }

        [Pure]
        public bool None(Func<T, bool> predicate)
            => !enumerable.Any(predicate);

        /// <summary>Takes all elements until (and including the first matching element).</summary>
        [Pure]
        public IEnumerable<T> TakeUntil(Func<T, bool> predicate)
        {
            var match = false;

            foreach (T element in enumerable)
            {
                if (match)
                {
                    break;
                }
                match = predicate(element);

                yield return element;
            }
        }

        [Pure]
        public T MinBy<TComparable>(Func<T, TComparable> getValue)
        where TComparable : IComparable<TComparable>
        {
            var result = enumerable.First();

            foreach (var item in enumerable.Skip(1))
            {
                var value = getValue(item);

                if (value.CompareTo(value) < 0)
                {
                    result = item;
                }
            }
            return result;
        }
    }

    extension<T>(IEnumerable<T> enumerable) where T : struct
    {
        /// <summary>Returns the first matching element, or null if no match was found.</summary>
        [Pure]
        public T? FirstOrNone()
        {
            using var enumerator = enumerable.GetEnumerator();
            return enumerator.MoveNext()
                ? enumerator.Current
                : null;
        }

        /// <summary>Returns the first matching element, or null if no match was found.</summary>
        [Pure]
        public T? FirstOrNone(Predicate<T> predicate)
        {
            foreach (var element in enumerable)
            {
                if (predicate(element))
                {
                    return element;
                }
            }
            return null;
        }
    }
}
