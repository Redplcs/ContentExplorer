namespace CommandoTools.ContentExplorer.Formats.Pak;

public class PakReader : IDisposable
{
	private readonly BinaryReader _reader;
	private readonly PakCompressionType _compressionType;
	private readonly PakVersion _version;
	private readonly int _fileCount;
	private readonly PakFileMetadata[] _metadata;
	private bool _disposed;

	public PakReader(Stream archiveStream) : this(archiveStream, dontRead: false)
	{
	}

	/// <summary>
	/// For testing purposes.
	/// </summary>
	internal PakReader(Stream archiveStream, bool dontRead)
	{
		_reader = new BinaryReader(archiveStream);

		if (!dontRead)
		{
			ThrowIfSignatureDoesNotMatch();

			_compressionType = ReadCompressionType();
			_version = ReadVersion();

			// Skip unknown value that always 1.
			// Even in the game this value does nothing.
			_reader.BaseStream.Seek(4, SeekOrigin.Current);

			_fileCount = ReadFileCount();
			_metadata = new PakFileMetadata[_fileCount];

			for (var i = 0; i < _fileCount; i++)
			{
				_metadata[i] = ReadFileMetadata();
			}
		}
	}

	internal void ThrowIfSignatureDoesNotMatch()
	{
		Span<byte> buffer = stackalloc byte[PakHeader.Signature.Length];

		_reader.Read(buffer);

		for (var i = 0; i < buffer.Length; i++)
		{
			var signatureByte = PakHeader.Signature[i];
			var bufferByte = buffer[i];

			if (signatureByte != bufferByte)
				throw new InvalidDataException("The signature does not match");
		}
	}

	internal PakCompressionType ReadCompressionType()
	{
		return (PakCompressionType)_reader.ReadByte();
	}

	internal PakVersion ReadVersion()
	{
		return (PakVersion)_reader.ReadInt32();
	}

	internal int ReadFileCount()
	{
		return _reader.ReadInt32();
	}

	internal PakFileMetadata ReadFileMetadata()
	{
		return new PakFileMetadata
		{
			Path = _reader.ReadNullTerminatedString(),
			DataOffset = _reader.ReadInt32(),
			Length = _reader.ReadInt32(),
			Unknown = _reader.ReadUInt64(),
		};
	}

	public PakCompressionType CompressionType => _compressionType;
	public PakVersion Version => _version;
	public int FileCount => _fileCount;

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_reader.Dispose();
			}

			_disposed = true;
		}
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(disposing: true);
	}
}
