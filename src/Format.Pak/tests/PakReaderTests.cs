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
		using var stream = new MemoryStream(buffer: GetBytesFromHeader(compressionType: expectedCompressionType));
		using var reader = new PakReader(stream);

		var compressionType = reader.CompressionType;

		Assert.Equal(expectedCompressionType, compressionType);
	}

	[Theory]
	[InlineData(PakVersion.Prototype)]
	[InlineData(PakVersion.Trial)]
	[InlineData(PakVersion.Release)]
	public void PakReader_WhenReadingVersionData_VersionIsAsExpected(PakVersion expectedVersion)
	{
		using var stream = new MemoryStream(buffer: GetBytesFromHeader(version: expectedVersion));
		using var reader = new PakReader(stream);

		var version = reader.Version;

		Assert.Equal(expectedVersion, version);
	}

	private static byte[] GetBytesFromHeader(PakCompressionType compressionType = PakCompressionType.None, PakVersion version = PakVersion.None)
	{
		var buffer = new byte[12];

		buffer[0] = (byte)'P';
		buffer[1] = (byte)'A';
		buffer[2] = (byte)'K';
		buffer[3] = (byte)compressionType;
		buffer[4] = (byte)version;
		buffer[5] = 0;
		buffer[6] = 0;
		buffer[7] = 0;
		buffer[8] = 1;
		buffer[9] = 0;
		buffer[10] = 0;
		buffer[11] = 0;

		return buffer;
	}
}
