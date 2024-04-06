namespace AnkiPoetry.Engine;

public static class Chunker
{
    public static IEnumerable<Chunk> Run(MyDocument doc, int chunk_size)
    {
        foreach (var section in doc.Sections)
        {
            foreach (var song in Augmented(section.Songs))
            {
                var screen_number = 1;

                foreach (var chunk in ChunksWithOverlap(song.Lines, chunk_size))
                {
                    var header = CreateHeader(section, song, screen_number);
                    yield return new(doc.MaxSongNumber, header, section.SectionNumber, song.SongNumber, chunk);
                    screen_number++;
                }
            }
        }
    }

    private static IEnumerable<MySong> Augmented(MySong[] songs)
    {
        for (var i = 0; i < songs.Length; ++i)
        {
            var lines = new List<MyLine>();

            var textBegin = ((i != 0) ? songs[i - 1].Lines.Last().Text : "");
            lines.Add(new(0, textBegin, LineType.Prev));

            lines.AddRange(songs[i].Lines);

            var new_num = songs[i].Lines.Max(a => a.LineNumber) + 1;

            var textEnd = ((i != songs.Length - 1) ? songs[i + 1].Lines.First().Text : "");
            lines.Add(new(new_num, textEnd, LineType.Next));

            yield return songs[i] with { Lines = [.. lines] };
        }
    }

    private static IEnumerable<TItem[]> ChunksWithOverlap<TItem>(IEnumerable<TItem> lines, int chunk_size)
    {
        var result = new List<TItem>();

        foreach (var line in lines)
        {
            result.Add(line);

            if (result.Count == chunk_size)
            {
                yield return result.ToArray();

                result.Clear();
                result.Add(line);
            }
        }

        if (result.Count > 0)
            yield return result.ToArray();
    }

    private static string CreateHeader(MySection section, MySong song, int screenNumber)
        => $"{section.SectionName}, {song.SongName} ({screenNumber})";
}

public record Chunk(int MaxSongNumber, string Header, int SectionNumber, int SongNumber, MyLine[] Lines);
