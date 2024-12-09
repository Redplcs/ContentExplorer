namespace CommandoTools.ContentExplorer.Formats.Pak.Tests;

public class PakReaderTests
{
	[Fact]
	public void PakReader_WhenSignatureDoesNotMatch_ThrowsInvalidDataException()
	{
		using var stream = new MemoryStream(buffer: [0xFF, 0xFF, 0xFF]);

		void Act()
		{
			var reader = new PakReader(stream);
		}

		Assert.Throws<InvalidDataException>(Act);
	}

	[Fact]
	public void PakReader_WhenStreamIsSmallerThanSignature_ThrowsInvalidDataException()
	{
		using var stream = new MemoryStream(buffer: [0x50]);

		void Act()
		{
			var reader = new PakReader(stream);
		}

		Assert.Throws<InvalidDataException>(Act);
	}

	[Fact]
	public void PakReader_WhenFourthByteIsA_CompressionTypeIsNone()
	{
		var expectedCompressionType = PakCompressionType.None;
		using var stream = new MemoryStream(buffer: [0x50, 0x41, 0x4B, 0x41]);
		var reader = new PakReader(stream);

		var compressionType = reader.CompressionType;

		Assert.Equal(expectedCompressionType, compressionType);
	}
}
