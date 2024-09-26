using System.Buffers.Binary;

namespace System.IO;

internal static class BinaryReaderExtensions
{
    public static int ReadInt32BigEndian(this BinaryReader reader)
        => BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(sizeof(int)));
}
