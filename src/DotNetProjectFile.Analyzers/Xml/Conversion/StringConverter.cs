using System.ComponentModel;
using System.Globalization;

namespace DotNetProjectFile.Xml.Conversion;

internal sealed class StringConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string)
        || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        => value is null || value is string
        ? value as string
        : base.ConvertFrom(context, culture, value);
}
