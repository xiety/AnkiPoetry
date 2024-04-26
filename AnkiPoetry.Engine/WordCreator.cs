using System.Text;
using System.Text.RegularExpressions;

namespace AnkiPoetry.Engine;

public partial class WordCreator : BaseCreatorPerLine<Card>
{
    protected override IEnumerable<Card> CreateCard(string number, string beginning, MyLine to, int colors, bool line_numbers)
    {
        if (to.LineType != LineType.Norm)
            yield break;

        var cloze = GetLineText(MakeCloze(to.Text), to, colors, line_numbers);
        var text = beginning + cloze;

        yield return new(number, text);
    }

    private static string MakeCloze(string text)
    {
        var matches = RegexWord().Matches(text);

        var sb = new StringBuilder();

        var cloze_num = 1;
        var last_word_end = 0;

        foreach (var match in matches.Cast<Match>())
        {
            sb.Append(text[last_word_end..match.Index]);

            sb.Append($"{{{{c{cloze_num}::{match.Value}}}}}");

            cloze_num++;

            last_word_end = match.Index + match.Length;
        }

        sb.Append(text[last_word_end..]);

        return sb.ToString();
    }

    //this regex matches words that may contain apostrophes within them
    [GeneratedRegex(@"\b(?:'\b|\B'\B)?\w+(?:'\w+)*\b")]
    private static partial Regex RegexWord();
}
