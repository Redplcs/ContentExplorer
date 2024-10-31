namespace CommandoTools.ContentExplorer.FileFormats.Pak;

internal struct PakFileMetadata
{
	public string? Path;
	public uint DataOffset;
	public uint Length;
	public ulong Unknown;
}
