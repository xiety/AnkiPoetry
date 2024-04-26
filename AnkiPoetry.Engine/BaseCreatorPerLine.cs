using System.Text;

namespace AnkiPoetry.Engine;

public abstract class BaseCreatorPerLine<T> : BaseCreator<T>
{
    protected abstract IEnumerable<T> CreateCard(string number, string beginning, MyLine to, int colors, bool line_numbers);

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
        for (var i = 0; i < chunk.Lines.Length - 1; ++i)
        {
            var to = chunk.Lines[i + 1];
            var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, to.LineNumber);

            var beginning = CreateHeader(chunk.Header) + JoinLines(chunk.Lines[..(i + 1)], colors, line_numbers);

            var cards = CreateCard(number, beginning, to, colors, line_numbers);

            foreach (var card in cards)
                yield return card;
        }
    }
}
