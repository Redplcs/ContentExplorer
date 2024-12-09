namespace CommandoTools.ContentExplorer.Formats.Pak;

internal struct PakFileMetadata
{
	public string? Path;
	public int DataOffset;
	public int Length;
	public ulong Unknown;
}
