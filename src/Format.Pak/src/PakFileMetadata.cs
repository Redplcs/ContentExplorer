namespace CommandoTools.ContentExplorer.Format.Pak;

internal struct PakFileMetadata
{
	public string? Path;
	public int DataOffset;
	public int Length;
	public ulong Unknown;
}
