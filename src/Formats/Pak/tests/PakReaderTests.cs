namespace CommandoTools.ContentExplorer.Formats.Pak.Tests;

public class PakReaderTests
{
	[Fact]
	public void PakReader_WhenSignatureDoesNotMatch_ThrowsInvalidDataException()
	{
		using var stream = new MemoryStream(buffer: [ 0xFF, 0xFF, 0xFF ]);

		void Act()
		{
			var reader = new PakReader(stream);
		}

		Assert.Throws<InvalidDataException>(Act);
	}
}
