using DotNetProjectFile.Conversion;
using System.Collections.Frozen;
using System.ComponentModel;

namespace DotNetProjectFile.MsBuild.Conversion;

public static class TypeConverters
{
    public static T? TryConvert<T>(string? value)
    {
        try
        {
            if (value is { Length: > 0 })
            {
                return (T?)Get(typeof(T)).ConvertFromInvariantString(value);
            }
        }
        catch
        {
            // Conversion failed.
        }
        return default;
    }

    public static TypeConverter Get(Type type)
    {
        var not_null = Nullable.GetUnderlyingType(type) ?? type;
        return TypeStore.TryGetValue(not_null, out var custom)
            ? custom
            : TypeDescriptor.GetConverter(not_null);
    }

    private static readonly FrozenDictionary<Type, TypeConverter> TypeStore = new Dictionary<Type, TypeConverter>()
    {
        [typeof(string)] = new StringConverter(),
        [typeof(bool)] = new BooleanConverter(),
        [typeof(IOFile)] = new IOFileConverter(),
        [typeof(IODirectory)] = new IODirectoryConverter(),
        [typeof(LanguageVersion)] = new LanguageVersionConverter(),
        [typeof(SemVer)] = new SemVerConverter(),
    }
    .ToFrozenDictionary();
}
