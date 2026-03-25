using DotNetProjectFile.Conversion;
using System.ComponentModel;

namespace DotNetProjectFile.MsBuild.Conversion;

/// <summary>Convertion helper using <see cref="TypeConverter"/>s.</summary>
internal static class Converts
{
    /// <summary>Convert shte string.</summary>
    [Pure]
    public static T? String<T>(string? value) where T : struct
    {
        if (value is { Length: > 0 })
        {
            try
            {
                return (T)Get<T>().ConvertFromInvariantString(value);
            }
            catch
            {
                // Conversions might fail.
            }
        }
        return default;
    }

    [Pure]
    private static TypeConverter Get<T>() where T : struct
    {
        if (Store.TryGetValue(typeof(T), out var converter))
        {
            return converter;
        }
        lock (locker)
        {
            return Store.TryGetValue(typeof(T), out converter)
                ? converter
                : (Store[typeof(T)] = TypeDescriptor.GetConverter(typeof(T)));
        }
    }

    private static readonly Dictionary<Type, TypeConverter> Store = new()
    {
        [typeof(string)] = new StringConverter(),
        [typeof(bool)] = new BooleanConverter(),
        [typeof(LanguageVersion)] = new LanguageVersionConverter(),
    };

    private static readonly object locker = new();
}
