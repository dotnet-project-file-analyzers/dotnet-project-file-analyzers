using System.Collections.Concurrent;

namespace DotNetProjectFile.Caching;

/// <summary>
/// Caches the set of all enum values of a given type.
/// Needed because .netstandard does not support Enum.GetValues{T}.
/// </summary>
public static class EnumCache
{
    private static readonly ConcurrentDictionary<Type, object> cache = new();

    public static ImmutableArray<T> GetValues<T>()
        where T : Enum
        => (ImmutableArray<T>)cache.GetOrAdd(typeof(T), type =>
        {
            var values = Enum.GetValues(type);
            var result = new T[values.Length];

            for (var i = 0; i < values.Length; i++)
            {
                var value = (T)values.GetValue(i);
                result[i] = value;
            }

            return result.ToImmutableArray();
        });
}
