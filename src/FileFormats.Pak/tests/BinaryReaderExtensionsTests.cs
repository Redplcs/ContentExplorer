using System.Text;

namespace CommandoTools.ContentExplorer.FileFormats.Pak.Tests;

public class BinaryReaderExtensionsTests
{
	[Fact]
	public void ReadNullTerminatedString()
	{
		const string expectedValue = nameof(BinaryReaderExtensionsTests);
		var bytes = CreateNullTerminatedString(expectedValue);
		using var stream = new MemoryStream(bytes);
		using var reader = new BinaryReader(stream);

		var value = reader.ReadNullTerminatedString();

		Assert.NotNull(value);
		Assert.Equal(expectedValue, value);
	}

	private static byte[] CreateNullTerminatedString(string value, int? bufferSize = null, Encoding? encoding = null)
	{
		encoding ??= Encoding.UTF8;
		bufferSize ??= encoding.GetByteCount(value) + 1;

		var buffer = new byte[bufferSize.Value];
		_ = encoding.GetBytes(value, 0, value.Length, buffer, 0);
		return buffer;
	}
}
