using System.Text;

namespace AnkiPoetry.Engine;

public partial class PageCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        var from = chunk.Lines.First();

        var header = CreateHeader(chunk.Header);
        var number = CreateNumber(chunk.MaxSongNumber, chunk.SectionNumber, chunk.SongNumber, from.LineNumber);

        yield return CreateOddEven(chunk, parameters, header, number);
        yield return CreateFirstWord(chunk, parameters, header, number);
    }

    private Card CreateOddEven(Chunk chunk, Parameters parameters, string header, string number)
    {
        var sb = new StringBuilder();
        sb.Append(header);

        var even = true;

        foreach (var line in chunk.Lines)
        {
            var inner_text = line.Text;

            if (!line.NotMy && line.Text != String.Empty)
                inner_text = MakeCloze(even ? 1 : 2, line.Text);

            var text = AddLineNumber(line, inner_text, parameters);
            sb.Append(AddHr(line, text));

            even = !even;
        }

        return new(number, sb.ToString());
    }

    private Card CreateFirstWord(Chunk chunk, Parameters parameters, string header, string number)
    {
        var sb = new StringBuilder();
        sb.Append(header);

        foreach (var line in chunk.Lines)
        {
            var inner_text = line.Text;

            if (!line.NotMy && line.Text != String.Empty)
                inner_text = MakeClozeLeaveFirstWord(1, line.Text);

            var text = AddLineNumber(line, inner_text, parameters);
            sb.Append(AddHr(line, text));
        }

        return new(number + "_first", sb.ToString());
    }

    private static string AddHr(MyLine line, string text)
    {
        var ret = "";

        if (line.IsFirst)
            ret += "<hr>";

        ret += text;

        if (line.IsLast)
            ret += "<hr>";

        return ret;
    }

    private static string MakeCloze(int cloze_num, string text)
        => $"{{{{c{cloze_num}::{text}}}}}";

    private static string MakeClozeLeaveFirstWord(int cloze_num, string text)
    {
        var matches = Regexes.RegexWord().Matches(text);

        var n = matches.Count == 1
            ? matches[0].Index + matches[0].Length
            : matches[1].Index;

        return text[0..n] + $"{{{{c{cloze_num}::{text[n..]}}}}}";
    }
}
