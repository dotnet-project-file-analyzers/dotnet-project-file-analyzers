using DotNetProjectFile.Json;

namespace DotNetProjectFile.GlobalJson;

public static class GlobalJsonFile
{
    extension(JsonFile file)
    {
        public SdkNode? SdkNode
            => (file.Value as JsonObject)?.Property<JsonObject>("sdk") is { } sdk
            ? new SdkNode(sdk)
            : null;
    }
}
