using DotNetProjectFile.Conversion;
using System.ComponentModel;
using System.Reflection;

namespace DotNetProjectFile.MsBuild.Conversion;

internal sealed class TypeConverters
{
    public T? TryConvert<T>(string? value, Type classType, string propertyName)
    {
        try
        {
            if (value is { })
            {
                return (T?)Get(classType, propertyName!).ConvertFromInvariantString(value);
            }
        }
        catch
        {
            // Conversion failed.
        }
        return default;
    }

    public TypeConverter Get(Type classType, string propertyName)
    {
        var property = classType.GetProperty(propertyName);

        if (PropertyStore.TryGetValue(property, out TypeConverter converter))
        {
            return converter;
        }

        lock (locker)
        {
            if (!PropertyStore.TryGetValue(property, out converter))
            {
                if (property.GetCustomAttribute<TypeConverterAttribute>(true) is { } attr)
                {
                    converter = (TypeConverter)Activator.CreateInstance(Type.GetType(attr.ConverterTypeName));
                }
                else if (!TypeStore.TryGetValue(property.PropertyType, out converter))
                {
                    var type = Nullable.GetUnderlyingType(property.PropertyType)
                        ?? property.PropertyType;

                    converter = TypeDescriptor.GetConverter(type);
                    TypeStore[property.PropertyType] = converter;
                }
                PropertyStore[property] = converter;
            }
        }
        return converter;
    }

    private readonly Dictionary<PropertyInfo, TypeConverter> PropertyStore = [];
    private readonly Dictionary<Type, TypeConverter> TypeStore = new()
    {
        [typeof(string)] = new StringConverter(),
        [typeof(bool)] = new BooleanConverter(),
        [typeof(LanguageVersion)] = new LanguageVersionConverter(),
    };

    private readonly object locker = new();
}
