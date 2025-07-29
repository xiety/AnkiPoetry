using System.Text;

namespace AnkiPoetry.Engine;

public partial class SimpleCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        var from = chunk.Lines.First();
        var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, from.LineNumber);

        var sb = new StringBuilder();

        foreach (var line in chunk.Lines)
        {
            var text = GetLineText(line, line.Text, parameters);
            sb.Append(text);
        }

        yield return new(number, sb.ToString());
    }

    protected override string GetLineText(MyLine line, string text, Parameters parameters)
    {
        var number = GetLineNumber(line, parameters);
        return number + text + Environment.NewLine;
    }
}
