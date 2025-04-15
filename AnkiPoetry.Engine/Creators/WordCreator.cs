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

            if (!to.NotMy && to.LineType != LineType.NextSong && to.LineType != LineType.NextPage)
            {
                var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, to.LineNumber);

                var beginning = CreateHeader(chunk, parameters) + JoinLines(chunk.Lines[..i], parameters);

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
        var formatted = GetLineText(line, cloze, parameters);

        if (lineNext is not null && lineNext.Text != "" && !lineNext.NotMy)
        {
            var clozeNext = MakeClozeFirstWord(lineNext.Text, ref cloze_num);
            var formattedNext = GetLineText(lineNext, clozeNext, parameters);

            ending += formattedNext;

            if (lineNext.IsLast)
                ending += "<hr>";
        }

        return new(number, beginning + formatted + ending);
    }

    private static string MakeCloze(string text, ref int cloze_num)
    {
        var matches = Regexes.RegexWord().Matches(text);
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
        var matches = Regexes.RegexWord().Matches(text);
        var sb = new StringBuilder();

        if (matches.Count > 0)
        {
            var i = 0;
            var word = "";

            while (i < matches.Count && word.Count(char.IsLetter) < 4)
            {
                var match = matches[i];
                word = text[0..(match.Index + match.Length)];
                i++;
            }

            if (i < matches.Count)
                word += "...";

            sb.Append($"{{{{c{cloze_num}::{word}::word}}}}");

            cloze_num++;
        }

        return sb.ToString();
    }
}
