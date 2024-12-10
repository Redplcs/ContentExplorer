using System.Runtime.InteropServices;

namespace CommandoTools.ContentExplorer.Format.Pak.Tests;

public class PakReaderTests
{
	[Fact]
	public void PakReader_WhenSignatureDoesNotMatch_ThrowsInvalidDataException()
	{
		using var stream = new MemoryStream(TestingHeaderBytes.InvalidSignature);

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
		using var stream = new MemoryStream(TestingHeaderBytes.BuildBytes(expectedCompressionType));
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
		using var stream = new MemoryStream(TestingHeaderBytes.BuildBytes(expectedVersion));
		using var reader = new PakReader(stream);

		var version = reader.Version;

		Assert.Equal(expectedVersion, version);
	}

	[Theory]
	[InlineData(1)]
	public void PakReader_WhenReadingFileCountData_FileCountIsAsExpected(int expectedFileCount)
	{
		using var stream = new MemoryStream(TestingHeaderBytes.BuildBytes(expectedFileCount));
		using var reader = new PakReader(stream);

		var fileCount = reader.FileCount;

		Assert.Equal(expectedFileCount, fileCount);
	}

	[Fact]
	public void PakReader_WhenReadingMetadataBlock_MetadataIsSingle()
	{
		var metadata = new PakFileMetadata
		{
			Path = "FILE.DATA",
			Length = 512,
			Unknown = 123123,
		};
		using var stream = new MemoryStream(TestingHeaderBytes.BuildBytes(metadata));
		using var reader = new PakReader(stream);

		Assert.Single(reader.Metadata);
	}
}
