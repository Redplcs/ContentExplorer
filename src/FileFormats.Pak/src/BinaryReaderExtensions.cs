using System.Text;

namespace CommandoTools.ContentExplorer.FileFormats.Pak;

internal static class BinaryReaderExtensions
{
	private static readonly StringBuilder s_stringBuilder = new();

	public static string ReadNullTerminatedString(this BinaryReader reader)
	{
		s_stringBuilder.Clear();

		int read;
		while ((read = reader.Read()) is not '\0' or -1)
		{
			var character = (char)read;
			s_stringBuilder.Append(character);
		}

		return s_stringBuilder.ToString();
	}
}
