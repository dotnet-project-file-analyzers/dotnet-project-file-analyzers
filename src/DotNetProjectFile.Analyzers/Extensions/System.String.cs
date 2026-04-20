using DotNetProjectFile.Text;

namespace System;

internal static class StringExtensions
{
    extension(string? str)
    {
        public string? NullIfEmpty() => str is { Length: 0 } ? null : str;

        public bool Contains(string value, StringComparison comparisonType)
            => str is { } && str.IndexOf(value, comparisonType) != -1;

        /// <summary>Matches both strings ignoring casting.</summary>
        public bool IsMatch(string? other)
            => string.Equals(str, other, StringComparison.OrdinalIgnoreCase);

        /// <inheritdoc cref="NGramsCollection.Create(string?, int)" />
        public NGramsCollection GetNGrams(int n)
            => NGramsCollection.Create(str, n);
    }

    extension(string str)
    {
        public string TrimStart(string? other)
        {
            if (other is not { Length: > 0 })
            {
                return str;
            }

            var result = str;

            while (result.StartsWith(other))
            {
                result = result[other.Length..];
            }

            return result;
        }

        public string TrimEnd(string? other)
        {
            if (other is not { Length: > 0 })
            {
                return str;
            }

            var result = str;

            while (result.EndsWith(other))
            {
                result = result[..(str.Length - other.Length)];
            }

            return result;
        }
    }
}
