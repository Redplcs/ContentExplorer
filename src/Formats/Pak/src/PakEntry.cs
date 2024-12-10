namespace CommandoTools.ContentExplorer.Formats.Pak;

public sealed class PakEntry
{
	private PakFileMetadata _metadata;

	/// <summary>
	/// For testing purposes.
	/// </summary>
	internal PakEntry(PakFileMetadata metadata = default)
	{
		_metadata = metadata;
	}

	public string Path
	{
		get => _metadata.Path!;
		set
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

			_metadata.Path = value;
		}
	}

	public int Length
	{
		get => _metadata.Length;
	}
}
