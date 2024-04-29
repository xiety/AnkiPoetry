using System.Text;

namespace AnkiPoetry.Engine;

public abstract class BaseCreatorPerLine<T> : BaseCreator<T>
{
    protected abstract T CreateCard(string number, string beginning, string ending, MyLine to, int colors, bool line_numbers);

    protected virtual MyLine[] FilterLines(MyLine[] lines)
        => lines;

    protected static string JoinLines(MyLine[] list, int colors, bool line_numbers)
    {
        var sb = new StringBuilder();

        foreach (var line in list)
        {
            var text = GetLineText(line.Text, line, colors, line_numbers);
            sb.Append(text);
        }

        return sb.ToString();
    }

    protected override IEnumerable<T> CardFromChunk(Chunk chunk, int colors, bool line_numbers)
    {
        var filtered = FilterLines(chunk.Lines);

        for (var i = 0; i < filtered.Length - 1; ++i)
        {
            var to = filtered[i + 1];
            var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, to.LineNumber);

            var beginning = CreateHeader(chunk.Header) + JoinLines(filtered[..(i + 1)], colors, line_numbers);
            var ending = to.IsLast ? "<hr>" : "";

            var card = CreateCard(number, beginning, ending, to, colors, line_numbers);

            yield return card;
        }
    }
}
