namespace DotNetProjectFile.Json;

internal static class JsonChildren
{
    extension(JsonObject @object)
    {
        public TPropertyType? Property<TPropertyType>(string name)
            where TPropertyType : JsonValue
            => @object.Children
                .OfType<JsonProperty>()
                .Where(p => p.Key?.Text == name)
                .Select(p => p.Value)
                .OfType<TPropertyType>()
                .FirstOrDefault();
    }
}
