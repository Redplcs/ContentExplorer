namespace CommandoTools.ContentExplorer.Format.Pak.Tests;

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

	[Theory]
	[InlineData(PakCompressionType.NotCompressed)]
	[InlineData(PakCompressionType.Deflate)]
	public void PakReader_WhenReadingCompressionTypeData_CompressionTypeIsAsExpected(PakCompressionType expectedCompressionType)
	{
		using var stream = new MemoryStream(buffer: GetBytesFromHeader(expectedCompressionType));
		using var reader = new PakReader(stream);

		var compressionType = reader.CompressionType;

		Assert.Equal(expectedCompressionType, compressionType);
	}

	private static byte[] GetBytesFromHeader(PakCompressionType compressionType)
	{
		var buffer = new byte[4];

		buffer[0] = (byte)'P';
		buffer[1] = (byte)'A';
		buffer[2] = (byte)'K';
		buffer[3] = (byte)compressionType;

		return buffer;
	}
}
