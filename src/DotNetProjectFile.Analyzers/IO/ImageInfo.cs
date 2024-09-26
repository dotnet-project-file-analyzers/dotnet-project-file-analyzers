using System.IO;

namespace DotNetProjectFile.IO;

public sealed record ImageInfo
{
    public string? Type { get; init; }

    public int Width { get; init; }

    public int Height { get; init; }

    public long Size { get; init; }

    public static ImageInfo ReadPng(Stream image)
    {
        using var reader = new BinaryReader(image);

        if (!HasPngHeader()) return new() { Size = reader.BaseStream.Length };

        reader.BaseStream.Position = 16;

        return new()
        {
            Type = "PNG",
            Width = reader.ReadInt32BigEndian(),
            Height = reader.ReadInt32BigEndian(),
            Size = reader.BaseStream.Length,
        };

        bool HasPngHeader()
        {
            var header = reader.ReadBytes(8);

            return image.Length > 16 + 13
                && header[0] == 0x89
                && header[1] == 'P'
                && header[2] == 'N'
                && header[3] == 'G'
                && header[4] == '\r'
                && header[5] == '\n'
                && header[6] == 0x1A // SUB
                && header[7] == '\n';
        }
    }
}
