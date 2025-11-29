using System.Text;

namespace AnkiPoetry.Engine;

public abstract class BaseCreator<T>
{
    protected abstract IEnumerable<T> CardFromChunk(Chunk chunk, Parameters parameters);

    public virtual T[] Run(Chunk[] chunks, Parameters parameters)
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
            var text = GetLineText(line, line.Text, parameters);

            if (line.IsFirst)
                sb.Append("<hr>");

            sb.Append(text);

            if (line.IsLast)
                sb.Append("<hr>");
        }

        return sb.ToString();
    }

    protected virtual string CreateNumber(Chunk chunk, int lineNumber)
        => $"{chunk.SectionNumber:00}.{chunk.SongNumber:00}.{lineNumber:000}";

    protected static string CreateHeader(Chunk chunk, Parameters parameters)
    {
        var starsHtml = "";

        if (parameters.StarMode != StarMode.None)
        {
            var num = parameters.StarMode switch
            {
                StarMode.PerChunk => chunk.ChunkNumber,
                StarMode.PerSection => chunk.SongNumber - 1, //TODO: make song number from zero
                StarMode.PerSong => chunk.ScreenNumber,
                _ => throw new NotImplementedException(),
            };

            var stars = num % parameters.StarNumber;
            var color = (num / parameters.StarNumber) % parameters.Colors;

            var starsText = String.Join("", Enumerable.Range(0, parameters.StarNumber)
                .Select(a => a == stars ? "★" : "☆"));

            starsHtml = $" <span class=\"line{color}\">{starsText}</span>";
        }

        var hiddenInfo = $"<div class=\"hiddenInfo\">{chunk.ChunkNumber + 1}</div>";

        return $"{hiddenInfo}<div class=\"header\">{chunk.Header}{starsHtml}</div>";
    }

    //C# operator % returns negative numbers for negative x, so we need to adjust result
    static int Mod(int x, int m)
    {
        var r = x % m;
        return r < 0 ? r + m : r;
    }

    static string ColorLine(string text, int n, int colors)
        => $"<div class=\"line{Mod(n, colors)}\">" + text + "</div>";

    protected virtual string GetLineText(MyLine line, string text, Parameters parameters)
    {
        var number = GetLineNumber(line, parameters);

        return ColorLine(
            number + text,
            (line.NumberForColor - 1), //to make first (zero) line violet not red
            parameters.Colors);
    }

    protected static string GetLineNumber(MyLine line, Parameters parameters)
        => parameters.LineNumbers
            ? $"{(parameters.Continuous ? line.ContinuousNumber : line.LineNumber),3}. "
            : "";
}
