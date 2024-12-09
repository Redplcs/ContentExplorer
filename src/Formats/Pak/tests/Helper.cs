using System.Runtime.InteropServices;

namespace CommandoTools.ContentExplorer.Formats.Pak.Tests;

internal static class Helper
{
	public static byte[] FileMetadataToBytes(PakFileMetadata metadata)
	{
		var size = metadata.Path!.Length + 1 + 4 + 4 + 8;
		var bytes = new byte[size];

		for (var i = 0; i < metadata.Path.Length; i++)
		{
			bytes[i] = (byte)metadata.Path[i];
		}

		MemoryMarshal.Write(bytes.AsSpan()[^16..], metadata.DataOffset);
		MemoryMarshal.Write(bytes.AsSpan()[^12..], metadata.Length);
		MemoryMarshal.Write(bytes.AsSpan()[^8..], metadata.Unknown);

		return bytes;
	}
}
