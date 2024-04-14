namespace AnkiPoetry.Engine;

public abstract class BaseCreator<T>
{
    private static string ColorLine(string text, int n, int colors)
        => $"<div class=\"line{n % colors}\">" + text + "</div>";

    protected static string GetLineText(string text, MyLine line, int colors)
        => AddLineNumber(line.LineNumber, AddPrefixPostfix(text, line.LineType), colors);

    protected static string AddLineNumber(int lineNumber, string text, int colors)
        => ColorLine($"{lineNumber,3}. {text}", lineNumber, colors);

    protected static string AddPrefixPostfix(string text, LineType lineType)
        => lineType switch
        {
            LineType.Norm => text,
            LineType.Prev => (text == "" ? "" : $"{text} ") + "(Begin)",
            LineType.Next => "(End)" + (text == "" ? "" : $" {text}"),
            _ => throw new Exception(),
        };

    protected abstract IEnumerable<T> CardFromChunk(Chunk chunk, int colors);

    public T[] Run(Chunk[] chunks, int colors)
    {
        if (colors <= 0)
            colors = 1;

        return chunks.SelectMany(a => CardFromChunk(a, colors)).ToArray();
    }

    protected virtual string CreateNumber(int maxSongNumber, int sectionNumber, int songNumber, int lineNumber)
        => $"{sectionNumber:00}.{songNumber:00}.{lineNumber:000}";

    protected static string CreateHeader(string header)
        => $"<div class=\"header\">{header}</div><br>";
}
