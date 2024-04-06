namespace AnkiPoetry.Engine;

public static partial class LoaderText
{
    public static MyDocument LoadFile(string file, int word_wrap = -1)
        => LoadLines(File.ReadAllLines(file), word_wrap);

    public static MyDocument LoadLines(string[] lines, int word_wrap = -1)
    {
        var songs = new List<MySong>();
        var songLines = new List<MyLine>();
        var sections = new List<MySection>();
        var sectionName = "";
        var songName = "";
        var sectionNumber = 1;
        var songNumber = 1;
        var lineNumber = 1;

        foreach (var text in lines)
        {
            if (text == "")
                continue;

            if (text.StartsWith("##"))
            {
                if (songLines.Count > 0)
                {
                    songs.Add(new MySong(songNumber, songName, [.. songLines]));
                    songLines.Clear();
                    lineNumber = 1;
                    songNumber++;
                }

                songName = text[2..].Trim();
            }
            else if (text.StartsWith("#"))
            {
                if (songLines.Count > 0)
                {
                    songs.Add(new(songNumber, songName, [.. songLines]));
                    songLines.Clear();
                    lineNumber = 1;
                    songNumber++;
                }

                if (songs.Count > 0)
                {
                    sections.Add(new(sectionNumber, sectionName, [.. songs]));
                    songs.Clear();
                    songNumber = 1;
                    sectionNumber++;
                }

                sectionName = text[1..].Trim();
            }
            else
            {
                var wrapped = TextWrapper.Wrap(text, word_wrap);

                foreach (var wrap in wrapped)
                {
                    songLines.Add(new(lineNumber, wrap, LineType.Norm));
                    lineNumber++;
                }
            }
        }

        if (songLines.Count > 0)
            songs.Add(new(songNumber, songName, [.. songLines]));

        if (songs.Count > 0)
            sections.Add(new(sectionNumber, sectionName, [.. songs]));

        return new([.. sections], 100);
    }
}

public static class TextWrapper
{
    public static string[] Wrap(string text, int word_wrap)
    {
        if (word_wrap == -1 || text.Length <= word_wrap)
            return [text];

        var lines = WrapParts(text, word_wrap);

        return [..AddDots([.. lines])] ;
    }

    private static IEnumerable<string> WrapParts(string text, int word_wrap)
    {
        var items = Split(text);

        var c = "";

        foreach (var item in items)
        {
            var n = c + item;

            if (n.Length > word_wrap)
            {
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

    private static IEnumerable<string> Split(string text)
    {
        var currentPart = "";

        foreach (var c in text)
        {
            currentPart += c;

            if (!char.IsLetterOrDigit(c) && c != ' ')
            {
                if (currentPart != "")
                {
                    yield return currentPart;
                    currentPart = "";
                }
            }
        }

        if (currentPart != "")
            yield return currentPart;
    }
}
