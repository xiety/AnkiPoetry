namespace AnkiPoetry.Engine;

public class LineCreator : BaseCreatorPerLine<Card>
{
    protected override Card CreateCard(string number, string beginning, string ending, MyLine to, int colors, bool line_numbers)
    {
        var text = MakeCloze(AddPrefixPostfix(to.Text, to.LineType));
        var cloze = AddLineNumber(to.LineNumber, text, colors, line_numbers);
        return new(number, beginning + cloze + ending);
    }

    private static string MakeCloze(string text)
        => $"{{{{c1::{text}}}}}";
}
