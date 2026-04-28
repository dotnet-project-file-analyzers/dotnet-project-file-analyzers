using DotNetProjectFile.Conversion;
using System.Collections.Frozen;
using System.ComponentModel;

namespace DotNetProjectFile.MsBuild.Conversion;

public static class TypeConverters
{
    public static T? TryConvert<T>(string? value)
    {
        var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        try
        {
            if (value is { Length: > 0 })
            {
                return (T?)Get(type).ConvertFromInvariantString(value);
            }
        }
        catch
        {
            // Conversion failed.
        }
        return default;
    }

    public static TypeConverter Get(Type type)
        => TypeStore.TryGetValue(type, out var custom)
        ? custom
        : TypeDescriptor.GetConverter(type);

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
