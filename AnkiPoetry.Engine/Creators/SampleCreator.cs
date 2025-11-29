using System.Text;

namespace AnkiPoetry.Engine;

public partial class SampleCreator : BaseCreator<Card>
{
    protected override IEnumerable<Card> CardFromChunk(Chunk chunk, Parameters parameters)
    {
        var from = chunk.Lines.First();
        var number = CreateNumber(chunk, from.LineNumber);

        var sb = new StringBuilder();

        var header = CreateHeader(chunk, parameters);
        sb.Append(header);

        foreach (var line in chunk.Lines)
        {
            if (line.IsFirst)
                sb.Append("<hr>");

            var text = GetLineText(line, line.Text, parameters);

            sb.Append(text);

            if (line.IsLast)
                sb.Append("<hr>");
        }

        yield return new(number, sb.ToString());
    }
}
