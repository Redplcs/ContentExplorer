namespace CommandoTools.ContentExplorer.Formats.Pak;

public class PakReader
{
	private readonly BinaryReader _reader;

	public PakReader(Stream archiveStream)
	{
		_reader = new BinaryReader(archiveStream);

		ThrowIfSignatureDoesNotMatch();
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
}
