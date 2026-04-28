using System.ComponentModel;
using System.Globalization;

namespace DotNetProjectFile.Conversion;

/// <summary>Implements a <see cref="TypeConverter"/> for <see cref="SemVer"/>.</summary>
internal sealed class SemVerConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value) => value switch
    {
        null => null,
        string str => SemVer.TryParse(str),
        _ => base.ConvertFrom(context, culture, value),
    };
}
