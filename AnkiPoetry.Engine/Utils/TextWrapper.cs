namespace AnkiPoetry.Engine;

public static class TextWrapper
{
    public static string[] Wrap(string text, Parameters parameters)
    {
        if (parameters.WordWrap == -1 || text.Length <= parameters.WordWrap)
            return [text];

        var lines = WrapOnSpaces(text, parameters.WordWrap).ToArray();

        return parameters.AddDots
            ? AddDots(lines).ToArray()
            : lines;
    }

    private static IEnumerable<string> WrapOnSpaces(string text, int word_wrap)
    {
        var items = text.Split(' ');

        var c = "";

        foreach (var item in items)
        {
            var n = (c == "") ? item : c + " " + item;

            if (n.Length > word_wrap)
            {
                if (c != "")
                    yield return c.Trim();

                c = item;
            }
            else
            {
                c = n;
            }
        }

        if (c != "")
            yield return c.Trim();
    }

    private static IEnumerable<string> AddDots(string[] lines)
    {
        if (lines.Length == 1)
        {
            yield return lines[0];
        }
        else
        {
            foreach (var (line, index) in lines.Indexed())
            {
                if (index == 0)
                    yield return $"{line}..";
                else if (index == lines.Length - 1)
                    yield return $"..{line}";
                else
                    yield return $"..{line}..";
            }
        }
    }
}
