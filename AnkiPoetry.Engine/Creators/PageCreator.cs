using System.Text;

namespace AnkiPoetry.Engine;

public partial class PageCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        var header = CreateHeader(chunk, parameters);
        var number = CreateNumber(chunk, 0);
        yield return Create(chunk, parameters, header, number);
    }

    protected override string CreateNumber(Chunk chunk, int lineNumber)
        => $"{chunk.SectionNumber:00}.{chunk.SongNumber:00}.{(chunk.ScreenNumber + 1):000}";

    protected Card Create(Chunk chunk, Parameters parameters, string header, string number)
    {
        var sb = new StringBuilder();
        sb.Append(header);

        foreach (var (index, line) in chunk.Lines.Index())
        {
            var inner_text = line.Text;

            if (!line.NotMy && !String.IsNullOrEmpty(line.Text))
                inner_text = CreateAllInOne(index, line);

            var text = GetLineText(line, inner_text, parameters);
            sb.Append(AddHr(line, text));
        }

        return new(number, sb.ToString());
    }

    private static string CreateAllInOne(int index, MyLine line)
    {
        var text = line.Text;
        var n = FirstWordIndex(text);

        if (index > 0)
        {
            text = text[0..n] + MakeCloze(3, text[n..]);
            text = MakeCloze(4, text);
            return MakeCloze(index % 2 == 0 ? 1 : 2, text);
        }
        else
        {
            return MakeCloze(index % 2 == 0 ? 1 : 2, text);
        }
    }

    private static string AddHr(MyLine line, string text) =>
       $"{(line.IsFirst ? "<hr>" : "")}{text}{(line.IsLast ? "<hr>" : "")}";

    private static string MakeCloze(int cloze_num, string text)
        => $"{{{{c{cloze_num}::{text}}}}}";

    private static int FirstWordIndex(string text)
    {
        var matches = Regexes.RegexWord().Matches(text);
        var n = matches[0].Index + 1;
        return n;
    }
}
