using System.Collections.Immutable;

namespace CommandoTools.ContentExplorer.FileFormats.Pak;

internal struct PakHeader
{
	public static readonly ImmutableArray<byte> Signature = "PAK"u8.ToImmutableArray();

	public PakCompression Compression;
	public PakVersion Version;
	public uint Unknown;
	public uint FileCount;
	public IList<PakFileMetadata> FileMetadata;
}
