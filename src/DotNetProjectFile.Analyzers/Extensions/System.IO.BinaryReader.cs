using System.Buffers.Binary;

namespace System.IO;

internal static class BinaryReaderExtensions
{
    extension(BinaryReader reader)
    {
        public int ReadInt32BigEndian()
            => BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(sizeof(int)));
    }
}
