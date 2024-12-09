namespace CommandoTools.ContentExplorer.Formats.Pak;

public class PakReader : IDisposable
{
	private readonly BinaryReader _reader;
	private readonly PakCompressionType _compressionType;
	private readonly PakVersion _version;
	private readonly int _fileCount;
	private bool _disposed;

	public PakReader(Stream archiveStream)
	{
		_reader = new BinaryReader(archiveStream);

		ThrowIfSignatureDoesNotMatch();

		_compressionType = (PakCompressionType)_reader.ReadByte();
		_version = (PakVersion)_reader.ReadInt32();

		// Skip unknown value that always 1.
		// Even in the game this value does nothing.
		_reader.BaseStream.Seek(4, SeekOrigin.Current);

		_fileCount = _reader.ReadInt32();
	}

	private void ThrowIfSignatureDoesNotMatch()
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
