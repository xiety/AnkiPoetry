using System.Text;

namespace AnkiPoetry.Engine;

public partial class SampleCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, int colors)
    {
        var from = chunk.Lines.First();
        var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, from.LineNumber);

        var sb = new StringBuilder();

        var header = CreateHeader(chunk.Header);
        sb.Append(header);

        foreach (var line in chunk.Lines)
        {
            var text = GetLineText(line.Text, line, colors);
            sb.Append(text);
        }

        yield return new(number, sb.ToString());
    }
}
