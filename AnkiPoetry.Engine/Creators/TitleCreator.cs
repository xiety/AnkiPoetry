using System.Text;

namespace AnkiPoetry.Engine;

public partial class TitleCreator : PageCreator
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

        return new MyDocument(sections);
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

    protected override string CreateNumber(Chunk chunk, int lineNumber)
        => $"title_{chunk.SectionNumber:00}.{(chunk.ScreenNumber + 1):000}";
}
