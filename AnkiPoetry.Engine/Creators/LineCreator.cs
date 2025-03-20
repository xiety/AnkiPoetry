namespace AnkiPoetry.Engine;

public class LineCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        for (var i = 0; i < chunk.Lines.Length - 1; ++i)
        {
            var to = chunk.Lines[i + 1];

            if (!to.NotMy && to.LineType != LineType.NextPage)
            {
                var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, to.LineNumber);

                var beginning = CreateHeader(chunk, parameters) + JoinLines(chunk.Lines[..(i + 1)], parameters);

                if (to.IsFirst)
                    beginning += "<hr>";

                var ending = to.IsLast ? "<hr>" : "";

                var card = CreateCard(number, beginning, ending, to, parameters);

                yield return card;
            }
        }
    }

    protected Card CreateCard(string number, string beginning, string ending, MyLine to, Parameters parameters)
    {
        var text = MakeCloze(to.Text);
        var cloze = AddLineNumber(to, text, parameters);
        return new(number, beginning + cloze + ending);
    }

    private static string MakeCloze(string text)
        => $"{{{{c1::{text}}}}}";
}
