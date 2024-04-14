using System.Text;

namespace AnkiPoetry.Engine;

public partial class PageCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, int colors)
    {
        var from = chunk.Lines.First();
        var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, from.LineNumber);

        var sb = new StringBuilder();

        var header = CreateHeader(chunk.Header);
        sb.Append(header);

        var even = true;

        foreach (var line in chunk.Lines)
        {
            var cloze_num = even ? 1 : 2;

            var line_text = AddPrefixPostfix(line.Text, line.LineType);

            var text = AddLineNumber(line.LineNumber, MakeCloze(cloze_num, line_text), colors);

            sb.Append(text);

            even = !even;
        }

        yield return new(number, sb.ToString());
    }

    private static string MakeCloze(int cloze_num, string text)
        => $"{{{{c{cloze_num}::{text}}}}}";
}
