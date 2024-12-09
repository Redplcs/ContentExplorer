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

	[Theory]
	[InlineData(0x41, PakCompressionType.None)]
	[InlineData(0x43, PakCompressionType.Deflate)]
	public void PakReader_WhenReadingCompressionTypeData_ReturnsExpectedCompressionType(byte representation, PakCompressionType expectedCompressionType)
	{
		using var stream = new MemoryStream(buffer: [representation]);
		using var reader = new PakReader(stream, dontRead: true);

		var compressionType = reader.ReadCompressionType();

		Assert.Equal(expectedCompressionType, compressionType);
	}

	[Theory]
	[InlineData(0x3, PakVersion.Prototype)]
	[InlineData(0x4, PakVersion.Trial)]
	[InlineData(0x5, PakVersion.Release)]
	public void PakReader_WhenReadingVersionData_ReturnsExpectedVersion(int representation, PakVersion expectedVersion)
	{
		using var stream = new MemoryStream(buffer: BitConverter.GetBytes(representation));
		using var reader = new PakReader(stream, dontRead: true);

		var version = reader.ReadVersion();

		Assert.Equal(expectedVersion, version);
	}

	[Theory]
	[InlineData(1)]
	public void PakReader_WhenReadingFileCountData_ReturnsExpectedFileCount(int expectedFileCount)
	{
		using var stream = new MemoryStream(buffer: BitConverter.GetBytes(expectedFileCount));
		using var reader = new PakReader(stream, dontRead: true);

		var fileCount = reader.ReadFileCount();

		Assert.Equal(expectedFileCount, fileCount);
	}

	[Fact]
	public void PakReader_WhenReadingFileMetadataData_ReturnsExpectedFileMetadata()
	{
		var expectedMetadata = new PakFileMetadata
		{
			Path = "FILE.DATA",
			DataOffset = 0,
			Length = 256,
			Unknown = 0,
		};
		using var stream = new MemoryStream(buffer: Helper.FileMetadataToBytes(expectedMetadata));
		using var reader = new PakReader(stream, dontRead: true);

		var metadata = reader.ReadFileMetadata();

		Assert.Equal(expectedMetadata, metadata);
	}
}
