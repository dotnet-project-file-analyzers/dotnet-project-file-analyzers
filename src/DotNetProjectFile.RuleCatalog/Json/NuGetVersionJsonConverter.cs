using NuGet.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNetProjectFile.RuleCatalog.Json;

internal sealed class NuGetVersionJsonConverter : JsonConverter<NuGetVersion?>
{
    /// <inheritdoc />
    public override NuGetVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => NuGetVersion.Parse(reader.GetString()!),
                JsonTokenType.Null => null,
                _ => throw new JsonException($"Unexpected token parsing {typeToConvert?.FullName}. {reader.TokenType} is not supported."),
            };
        }
        catch (Exception x) when (x is not JsonException)
        {
            throw new JsonException(x.Message, x);
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, NuGetVersion? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
