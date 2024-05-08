using System.Text;

namespace AnkiPoetry.Engine;

public abstract class BaseCreatorPerLine<T> : BaseCreator<T>
{
    protected abstract T CreateCard(string number, string beginning, string ending, MyLine to, Parameters parameters);

    protected virtual MyLine[] FilterLines(MyLine[] lines)
        => lines;

    protected string JoinLines(MyLine[] list, Parameters parameters)
    {
        var sb = new StringBuilder();

        foreach (var line in list)
        {
            var text = GetLineText(line.Text, line, parameters);
            sb.Append(text);

            if (line.IsLast)
                sb.Append("<hr>");
        }

        return sb.ToString();
    }

    protected override IEnumerable<T> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        var filtered = FilterLines(chunk.Lines);

        for (var i = 0; i < filtered.Length - 1; ++i)
        {
            var to = filtered[i + 1];
            var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, to.LineNumber);

            var beginning = CreateHeader(chunk.Header) + JoinLines(filtered[..(i + 1)], parameters);
            var ending = to.IsLast ? "<hr>" : "";

            var card = CreateCard(number, beginning, ending, to, parameters);

            yield return card;
        }
    }
}
