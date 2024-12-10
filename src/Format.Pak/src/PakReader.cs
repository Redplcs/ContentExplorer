using System.Collections.Immutable;

namespace CommandoTools.ContentExplorer.Format.Pak;

public sealed class PakReader : IDisposable
{
	private static readonly ImmutableArray<byte> s_signature = "PAK"u8.ToImmutableArray();

	private readonly BinaryReader _binaryReader;
	private readonly PakCompressionType _compressionType;

	public PakReader(Stream archiveStream)
	{
		_binaryReader = new BinaryReader(archiveStream);

		ThrowIfSignatureDoesNotMatch();

		_compressionType = (PakCompressionType)_binaryReader.ReadByte();
	}

	private void ThrowIfSignatureDoesNotMatch()
	{
		Span<byte> buffer = stackalloc byte[s_signature.Length];

		_binaryReader.Read(buffer);

		for (var i = 0; i < s_signature.Length; i++)
		{
			var signatureByte = s_signature[i];
			var bufferByte = buffer[i];

			if (signatureByte != bufferByte)
			{
				throw new InvalidDataException("The signature does not match");
			}
		}
	}

	public PakCompressionType CompressionType => _compressionType;

	public void Dispose()
	{
		_binaryReader.Dispose();
	}
}
