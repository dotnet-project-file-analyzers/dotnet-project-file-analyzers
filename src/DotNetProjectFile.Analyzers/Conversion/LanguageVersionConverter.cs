using System.ComponentModel;
using System.Globalization;

namespace DotNetProjectFile.Conversion;

/// <summary>Implements a <see cref="TypeConverter"/> for <see cref="LanguageVersion"/>.</summary>
public sealed class LanguageVersionConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string);

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        => value is string str
        ? LanguageVersion.Parse(str)
        : LanguageVersion.None;
}
