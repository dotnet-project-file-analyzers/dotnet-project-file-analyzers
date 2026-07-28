using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNetProjectFile.Diagnostics.Json;

internal sealed class StringArrayJsonConverter : JsonConverter<ImmutableArray<string>>
{
    /// <inheritdoc />
    public override ImmutableArray<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => [.. reader.GetString()!.Split(';')],
                JsonTokenType.Null => [],
                _ => throw new JsonException($"Unexpected token parsing {typeToConvert?.FullName}. {reader.TokenType} is not supported."),
            };
        }
        catch (Exception x) when (x is not JsonException)
        {
            throw new JsonException(x.Message, x);
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ImmutableArray<string> value, JsonSerializerOptions options)
    {
        if (value.Length is 0)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(string.Join(';', value));
        }
    }
}
