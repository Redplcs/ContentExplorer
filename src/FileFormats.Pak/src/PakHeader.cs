namespace CommandoTools.ContentExplorer.FileFormats.Pak;

internal struct PakHeader
{
	public PakCompression Compression;
	public PakVersion Version;
	public uint Unknown;
	public uint FileCount;
	public IList<PakFileMetadata> FileMetadata;
}
