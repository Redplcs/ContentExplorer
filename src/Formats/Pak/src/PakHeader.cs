using System.Collections.Immutable;

namespace CommandoTools.ContentExplorer.Formats.Pak;

internal readonly struct PakHeader
{
	public static readonly ImmutableArray<byte> Signature = "PAK"u8.ToImmutableArray();
}
