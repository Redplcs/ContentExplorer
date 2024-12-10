using System.Collections.Immutable;

namespace CommandoTools.ContentExplorer.Format.Pak;

public class PakReader
{
	private static readonly ImmutableArray<byte> s_signature = "PAK"u8.ToImmutableArray();

	private readonly BinaryReader _binaryReader;

	public PakReader(Stream archiveStream)
	{
		_binaryReader = new BinaryReader(archiveStream);

		ThrowIfSignatureDoesNotMatch();
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
}
