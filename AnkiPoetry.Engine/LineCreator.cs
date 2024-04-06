namespace AnkiPoetry.Engine;

public class LineCreator : BaseCreatorPerLine<Card>
{
    protected override IEnumerable<Card> CreateCard(string number, string beginning, MyLine to, int colors)
    {
        var cloze = GetLineText(MakeCloze(to.Text), to, colors);
        yield return new(number, beginning + cloze);
    }

    private static string MakeCloze(string text)
        => $"{{{{c1::{text}}}}}";
}
