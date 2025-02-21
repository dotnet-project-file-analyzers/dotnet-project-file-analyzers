using System.IO;
using System.IO.Compression;
using System.Text;

namespace DotNetProjectFile.Licensing;

public sealed record SpdxLicenseInfo
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required bool Osi { get; init; }

    public required bool Fsf { get; init; }

    public required ImmutableArray<string> SeeAlso { get; init; }

    public required ImmutableArray<string> LicenseTexts { get; init; }

    public void WriteTo(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(Name);
        writer.Write(Osi);
        writer.Write(Fsf);

        writer.Write(SeeAlso.Length);
        for (var i = 0; i < SeeAlso.Length; i++)
        {
            writer.Write(SeeAlso[i]);
        }

        writer.Write(LicenseTexts.Length);
        for (var i = 0; i < LicenseTexts.Length; i++)
        {
            writer.Write(LicenseTexts[i]);
        }
    }

    public static SpdxLicenseInfo ReadFrom(BinaryReader reader)
    {
        var id = reader.ReadString();
        var name = reader.ReadString();
        var osi = reader.ReadBoolean();
        var fsf = reader.ReadBoolean();

        var seeAlso = new string[reader.ReadInt32()];
        for (var i = 0; i < seeAlso.Length; i++)
        {
            seeAlso[i] = reader.ReadString();
        }

        var licenseTexts = new string[reader.ReadInt32()];
        for (var i = 0; i < licenseTexts.Length; i++)
        {
            licenseTexts[i] = reader.ReadString();
        }

        return new()
        {
            Id = id,
            Name = name,
            Osi = osi,
            Fsf = fsf,
            SeeAlso = [.. seeAlso],
            LicenseTexts = [.. licenseTexts],
        };
    }

    public static void WriteAllTo(IReadOnlyList<SpdxLicenseInfo> infos, BinaryWriter writer)
    {
        writer.Write(infos.Count);
        foreach (var info in infos)
        {
            info.WriteTo(writer);
        }
    }

    public static void WriteAllTo(IReadOnlyList<SpdxLicenseInfo> infos, Stream stream)
    {
        using var writer = new BinaryWriter(output: stream, encoding: Encoding.UTF8, leaveOpen: true);
        WriteAllTo(infos, writer);
        writer.Flush();
    }

    public static void WriteAllToCompressed(IReadOnlyList<SpdxLicenseInfo> infos, Stream stream)
    {
        using var compressor = new DeflateStream(stream: stream, CompressionMode.Compress, leaveOpen: true);
        WriteAllTo(infos, compressor);
        compressor.Flush();
    }

    public static SpdxLicenseInfo[] ReadAllFrom(BinaryReader reader)
    {
        var result = new SpdxLicenseInfo[reader.ReadInt32()];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = ReadFrom(reader);
        }
        return result;
    }

    public static SpdxLicenseInfo[] ReadAllFrom(Stream stream)
    {
        using var reader = new BinaryReader(input: stream, encoding: Encoding.UTF8, leaveOpen: true);
        return ReadAllFrom(reader);
    }

    public static SpdxLicenseInfo[] ReadAllFromCompressed(Stream stream)
    {
        using var decompressor = new DeflateStream(stream: stream, CompressionMode.Decompress, leaveOpen: true);
        return ReadAllFrom(decompressor);
    }
}
