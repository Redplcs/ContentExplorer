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
	public void PakReader_WhenCompressionTypeDataEqualsRepresentation_CompressionTypeMustBeAsExpected(byte representation, PakCompressionType expectedCompressionType)
	{
		using var stream = new MemoryStream(buffer: [0x50, 0x41, 0x4B, representation, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0]);
		using var reader = new PakReader(stream);

		var compressionType = reader.CompressionType;

		Assert.Equal(expectedCompressionType, compressionType);
	}

	[Theory]
	[InlineData(0x3, PakVersion.Prototype)]
	[InlineData(0x4, PakVersion.Trial)]
	[InlineData(0x5, PakVersion.Release)]
	public void PakReader_WhenVersionDataEqualsRepresentation_VersionMustBeAsExpected(byte representation, PakVersion expectedVersion)
	{
		using var stream = new MemoryStream(buffer: [0x50, 0x41, 0x4B, 0x41, representation, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0]);
		using var reader = new PakReader(stream);

		var version = reader.Version;

		Assert.Equal(expectedVersion, version);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public void PakReader_WhenFileCountDataHasValue_FileCountHasSameValue(byte expectedFileCount)
	{
		using var stream = new MemoryStream(buffer: [0x50, 0x41, 0x4B, 0x41, 0x5, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, expectedFileCount, 0x0, 0x0, 0x0]);
		using var reader = new PakReader(stream);

		var fileCount = reader.FileCount;

		Assert.Equal(expectedFileCount, fileCount);
	}
}
