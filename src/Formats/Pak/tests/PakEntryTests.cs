namespace CommandoTools.ContentExplorer.Formats.Pak.Tests;

public class PakEntryTests
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	[InlineData("\t")]
	[InlineData("\n")]
	public void Path_ForNullOrWhiteSpaceValues_ThrowsArgumentException(string? value)
	{
		var entry = new PakEntry();

		void Act()
		{
			entry.Path = value!;
		}

		Assert.ThrowsAny<ArgumentException>(Act);
	}
}
