namespace AnkiPoetry.Engine;

public class LineBackCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        var header = CreateHeader(chunk, parameters);

        for (var i = chunk.Lines.Length; i >= 1; --i)
        {
            var lineToRecall = chunk.Lines[i - 1];

            if (!lineToRecall.NotMy &&
                lineToRecall.LineType != LineType.PrevPage &&
                lineToRecall.LineType != LineType.NextPage &&
                lineToRecall.LineType != LineType.NextSong)
            {
                var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, lineToRecall.LineNumber);

                var beginning = header;

                var emptyLinesCount = i - 1;
                if (emptyLinesCount > 0)
                    beginning += string.Join("", Enumerable.Repeat("<br>", emptyLinesCount));

                if (lineToRecall.IsFirst)
                    beginning += "<hr>";

                var ending = lineToRecall.IsLast ? "<hr>" : "";

                ending += JoinLines(chunk.Lines[i..], parameters);

                var card = CreateCard(number, beginning, ending, lineToRecall, parameters);

                yield return card;
            }
        }
    }

    protected Card CreateCard(string number, string beginning, string ending, MyLine to, Parameters parameters)
    {
        var text = MakeCloze(to.Text);
        var cloze = GetLineText(to, text, parameters);
        return new(number, beginning + cloze + ending);
    }

    private static string MakeCloze(string text)
        => $"{{{{c1::{text}}}}}";
}
