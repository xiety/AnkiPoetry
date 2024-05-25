using System.Text;

namespace AnkiPoetry.Engine;

public abstract class BaseCreator<T>
{
    private static string ColorLine(string text, int n, int colors)
        => $"<div class=\"line{n % colors}\">" + text + "</div>";

    protected string GetLineText(string text, MyLine line, Parameters parameters)
        => AddLineNumber(line, text, parameters);

    protected string AddLineNumber(MyLine line, string text, Parameters parameters)
        => ColorLine((parameters.LineNumbers ? $"{(parameters.Continous ? line.ContinousNumber : line.LineNumber),3}. " : "") + text, line.LineNumber, parameters.Colors);

    protected abstract IEnumerable<T> CardFromChunk(Chunk chunk, Parameters parameters);

    public T[] Run(Chunk[] chunks, Parameters parameters)
    {
        if (parameters.Colors <= 0)
            parameters.Colors = 1;

        return chunks.SelectMany(a => CardFromChunk(a, parameters)).ToArray();
    }

    protected string JoinLines(MyLine[] list, Parameters parameters)
    {
        var sb = new StringBuilder();

        foreach (var line in list)
        {
            var text = GetLineText(line.Text, line, parameters);

            if (line.IsFirst)
                sb.Append("<hr>");

            sb.Append(text);

            if (line.IsLast)
                sb.Append("<hr>");
        }

        return sb.ToString();
    }

    protected virtual string CreateNumber(int maxSongNumber, int sectionNumber, int songNumber, int lineNumber)
        => $"{sectionNumber:00}.{songNumber:00}.{lineNumber:000}";

    protected static string CreateHeader(string header)
        => $"<div class=\"header\">{header}</div><br>";
}
