namespace AnkiPoetry.Engine;

public class LineCreator : BaseCreatorPerLine<Card>
{
    protected override Card CreateCard(string number, string beginning, string ending, MyLine to, Parameters parameters)
    {
        var text = MakeCloze(AddPrefixPostfix(to.Text, to.LineType));
        var cloze = AddLineNumber(to, text, parameters);
        return new(number, beginning + cloze + ending);
    }

    private static string MakeCloze(string text)
        => $"{{{{c1::{text}}}}}";
}
