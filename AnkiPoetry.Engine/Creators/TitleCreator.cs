using System.Text;

namespace AnkiPoetry.Engine;

public partial class TitleCreator : BaseCreator<Card>
{
    public override Card[] Run(Chunk[] chunks, Parameters parameters)
    {
        var titleParameters = parameters with
        {
            ChunkSize = parameters.StarNumber + 1,
            StarMode = StarMode.None,
        };

        var titleDoc = CreateTitlesDoc(chunks, titleParameters);
        var titleChunks = Chunker.Run(titleDoc, titleParameters)
            .Where(a => a.Lines.Length > 2).ToArray();

        return base.Run(titleChunks, titleParameters);
    }

    private static MyDocument CreateTitlesDoc(Chunk[] chunks, Parameters p)
    {
        var sections = chunks
            .GroupBy(c => new { c.SectionNumber, c.SectionName })
            .Select(g => new MySection(g.Key.SectionNumber, g.Key.SectionName, [CreateTitlesSong(g, p)]))
            .ToArray();

        return new MyDocument(sections, chunks.First().MaxSongNumber);
    }

    private static MySong CreateTitlesSong(IEnumerable<Chunk> chunks, Parameters parameters)
    {
        var songLines = chunks.Select((chunk, i) =>
            chunk.Lines.First(l => l.LineType == LineType.Norm && !l.NotMy) with
            {
                //To colorize each page in a single color
                NumberForColor = (i + parameters.StarNumber) / parameters.StarNumber,
                IsFirst = false,
                IsLast = false
            });

        return new(1, "Titles", [.. songLines]);
    }

    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        var header = CreateHeader(chunk, parameters);
        var number = $"title_{chunk.SectionNumber:00}.{(chunk.ScreenNumber + 1):000}";
        yield return Create(chunk, parameters, header, number, "", CreateAllInOne);
    }

    private Card Create(Chunk chunk, Parameters parameters, string header, string number, string suffix, Func<int, MyLine, string> func)
    {
        var sb = new StringBuilder();
        sb.Append(header);

        foreach (var (index, line) in chunk.Lines.Index())
        {
            var inner_text = line.Text;

            if (!line.NotMy && !String.IsNullOrEmpty(line.Text))
                inner_text = func(index, line);

            var text = GetLineText(line, inner_text, parameters);
            sb.Append(AddHr(line, text));
        }

        return new(number + suffix, sb.ToString());
    }

    private string CreateAllInOne(int index, MyLine line)
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
