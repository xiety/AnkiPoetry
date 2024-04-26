namespace AnkiPoetry.Engine;

public static partial class LoaderText
{
    public static MyDocument LoadText(string text, int word_wrap, bool wrap_on_spaces, bool add_dots)
    {
        var lines = text.Replace("\r\n", "\n").Split("\n");
        return LoadLines(lines, word_wrap, wrap_on_spaces, add_dots);
    }

    public static MyDocument LoadLines(string[] lines, int word_wrap, bool wrap_on_spaces, bool add_dots)
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
                songName = "";
            }
            else
            {
                var wrapped = TextWrapper.Wrap(text.Trim(), word_wrap, wrap_on_spaces, add_dots);

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
