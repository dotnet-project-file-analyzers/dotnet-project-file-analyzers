using DotNetProjectFile.Conversion;
using System.Collections.Frozen;
using System.ComponentModel;

namespace DotNetProjectFile.MsBuild.Conversion;

internal sealed class TypeConverters
{
    public T? TryConvert<T>(string? value)
    {
        try
        {
            if (value is { Length: > 0 })
            {
                var converter = TypeStore.TryGetValue(typeof(T), out var custom)
                    ? custom
                    : TypeDescriptor.GetConverter(typeof(T));

                return (T?)converter.ConvertFromInvariantString(value);
            }
        }
        catch
        {
            // Conversion failed.
        }
        return default;
    }

    private readonly FrozenDictionary<Type, TypeConverter> TypeStore = new Dictionary<Type, TypeConverter>()
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
