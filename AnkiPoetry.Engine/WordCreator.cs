using System.Text;
using System.Text.RegularExpressions;

namespace AnkiPoetry.Engine;

public partial class WordCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        for (var i = 1; i < chunk.Lines.Length; ++i)
        {
            var to = chunk.Lines[i];

            if (to.LineType != LineType.NextSong && to.LineType != LineType.NextPage)
            {
                var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, to.LineNumber);

                var beginning = CreateHeader(chunk.Header) + JoinLines(chunk.Lines[..i], parameters);

                if (to.IsFirst)
                    beginning += "<hr>";

                var ending = to.IsLast ? "<hr>" : "";

                var nextLine = i < chunk.Lines.Length - 1
                    ? chunk.Lines[i + 1]
                    : null;

                var card = CreateCard(number, beginning, ending, to, nextLine, parameters);

                yield return card;
            }
        }
    }

    protected Card CreateCard(string number, string beginning, string ending, MyLine line, MyLine? lineNext, Parameters parameters)
    {
        var cloze_num = 1;
        var cloze = MakeCloze(line.Text, ref cloze_num);
        var formatted = GetLineText(cloze, line, parameters);

        if (lineNext is not null && lineNext.Text != "")
        {
            var clozeNext = MakeClozeFirstWord(lineNext.Text, ref cloze_num);
            var formattedNext = GetLineText(clozeNext, lineNext, parameters);
            ending += formattedNext;
        }

        return new(number, beginning + formatted + ending);
    }

    private static string GetFirstWord(string text)
    {
        var matches = RegexWord().Matches(text);
        var match = matches[0];

        return text[0..(match.Index + match.Length)];
    }

    private static string MakeCloze(string text, ref int cloze_num)
    {
        var matches = RegexWord().Matches(text);
        var sb = new StringBuilder();
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

    private static string MakeClozeFirstWord(string text, ref int cloze_num)
    {
        var matches = RegexWord().Matches(text);
        var sb = new StringBuilder();
        var last_word_end = 0;

        foreach (var match in matches.Cast<Match>().Take(1))
        {
            sb.Append(text[last_word_end..match.Index]);
            sb.Append($"{{{{c{cloze_num}::{match.Value}::word}}}}");

            cloze_num++;

            last_word_end = match.Index + match.Length;
        }

        return sb.ToString();
    }

    //this regex matches words that may contain apostrophes within them
    [GeneratedRegex(@"\b(?:'\b|\B'\B)?\w+(?:'\w+)*\b")]
    private static partial Regex RegexWord();
}
