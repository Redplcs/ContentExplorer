using System.Text;

namespace CommandoTools.ContentExplorer.Format.Pak.Tests;

internal static class TestingHeaderBytes
{
	public static readonly byte[] InvalidSignature = [0xFF, 0xFF, 0xFF];

	public static byte[] BuildBytes(PakCompressionType compressionType)
	{
		return BuildBytes(compressionType, PakVersion.Release, fileCount: 0);
	}

	public static byte[] BuildBytes(PakVersion version)
	{
		return BuildBytes(PakCompressionType.NotCompressed, version, fileCount: 0);
	}

	public static byte[] BuildBytes(int fileCount)
	{
		return BuildBytes(PakCompressionType.NotCompressed, PakVersion.Release, fileCount, new PakFileMetadata[fileCount]);
	}

	public static byte[] BuildBytes(params PakFileMetadata[] metadata)
	{
		return BuildBytes(PakCompressionType.NotCompressed, PakVersion.Release, fileCount: metadata.Length, metadata);
	}

	public static byte[] BuildBytes(PakCompressionType compressionType, PakVersion version, int fileCount, params PakFileMetadata[] metadata)
	{
		var bytes = new List<byte>();

		bytes.AddRange("PAK"u8);
		bytes.AddRange((byte)compressionType);
		bytes.AddRange(BitConverter.GetBytes((int)version));
		bytes.AddRange(BitConverter.GetBytes(1));
		bytes.AddRange(BitConverter.GetBytes(fileCount));

		foreach (var item in metadata)
		{
			bytes.AddRange(!string.IsNullOrWhiteSpace(item.Path) ? Encoding.ASCII.GetBytes(item.Path) : "Null"u8);
			bytes.Add(0);
			bytes.AddRange(BitConverter.GetBytes(item.DataOffset));
			bytes.AddRange(BitConverter.GetBytes(item.Length));
			bytes.AddRange(BitConverter.GetBytes(item.Unknown));
		}

		return [.. bytes];
	}
}
