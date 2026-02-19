using System.ComponentModel;
using System.Globalization;

namespace DotNetProjectFile.CodeAnalysis;

/// <summary>
/// Used by NUnit for parsing parameterised tests.
/// </summary>
public sealed class LanguageTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        => value switch
        {
            string str => Language.Parse(str),
            _ => base.ConvertFrom(context, culture, value),
        };
}
