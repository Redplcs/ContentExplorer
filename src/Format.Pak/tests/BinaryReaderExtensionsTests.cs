using System.Text;

namespace CommandoTools.ContentExplorer.Format.Pak.Tests;

public class BinaryReaderExtensionsTests
{
	[Fact]
	public void ReadNullTerminatedString_ForStringEndsWithNull_ReturnsSameStringWithoutNullTerminator()
	{
		var expected = "String";
		var buffer = new byte[expected.Length + 1];
		Encoding.UTF8.GetBytes(expected, 0, expected.Length, buffer, 0);
		using var stream = new MemoryStream(buffer);
		using var reader = new BinaryReader(stream);

		var actual = reader.ReadNullTerminatedString();

		Assert.Equal(expected, actual);
	}
}
