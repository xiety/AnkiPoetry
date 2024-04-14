namespace AnkiPoetry.Engine;

public class LineCreator : BaseCreatorPerLine<Card>
{
    protected override IEnumerable<Card> CreateCard(string number, string beginning, MyLine to, int colors)
    {
        var text = MakeCloze(AddPrefixPostfix(to.Text, to.LineType));
        var cloze = AddLineNumber(to.LineNumber, text, colors);
        yield return new(number, beginning + cloze);
    }

    private static string MakeCloze(string text)
        => $"{{{{c1::{text}}}}}";
}
