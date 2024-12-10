using System.Text;

namespace CommandoTools.ContentExplorer.Format.Pak;

internal static class BinaryReaderExtensions
{
	private static readonly StringBuilder s_stringBuilder = new();

	public static string? ReadNullTerminatedString(this BinaryReader reader)
	{
		s_stringBuilder.Clear();

		for (var character = reader.Read(); character is not '\0' or -1; character = reader.Read())
		{
			s_stringBuilder.Append((char)character);
		}

		return s_stringBuilder.ToString();
	}
}
