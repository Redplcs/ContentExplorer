namespace CommandoTools.ContentExplorer.Format.Pak;

public sealed class PakEntry
{
	private PakFileMetadata _metadata;

	public string Path
	{
		get => _metadata.Path!;
		set
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

			_metadata.Path = value;
		}
	}
}
