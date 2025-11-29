namespace AnkiPoetry.Engine;

public static class Chunker
{
    public static Chunk[] Run(MyDocument doc, Parameters parameters)
        => RunEnumerable(doc, parameters).ToArray();

    private static IEnumerable<Chunk> RunEnumerable(MyDocument doc, Parameters parameters)
    {
        var chunk_number = 0;

        foreach (var section in doc.Sections)
        {
            foreach (var song in Augmented(section, parameters))
            {
                var screen_number = 0;

                foreach (var chunk in ChunksWithOverlap(song.Lines, parameters.ChunkSize))
                {
                    var header = CreateHeader(section, song, screen_number);
                    yield return new(header, section.SectionNumber, section.SectionName, song.SongNumber, screen_number, chunk_number, chunk);
                    screen_number++;
                    chunk_number++;
                }
            }
        }
    }

    private static IEnumerable<MySong> Augmented(MySection section, Parameters parameters)
    {
        for (var i = 0; i < section.Songs.Length; ++i)
        {
            var lines = new List<MyLine>();

            var song = section.Songs[i];
            var prev_song = i > 0 ? section.Songs[i - 1] : null;
            var next_song = i < section.Songs.Length - 1 ? section.Songs[i + 1] : null;

            var title = !String.IsNullOrEmpty(song.SongName) ? song.SongName : section.SectionName;

            if (prev_song is not null && parameters.OverlapChapters)
            {
                var prev_line = prev_song.Lines.Last();
                var text_begin = prev_line.Text;
                var continuous_num_begin = (parameters.Continuous ? prev_line.ContinuousNumber : prev_line.LineNumber);
                lines.Add(new(0, continuous_num_begin, 0, text_begin, LineType.PrevSong, false, false, false));
            }
            else
            {
                lines.Add(new(0, 0, 0, title, LineType.PrevSong, false, false, true));
            }

            var first = song.Lines[0];
            lines.Add(first with { IsFirst = true });

            if (song.Lines.Length > 1)
            {
                lines.AddRange(song.Lines[1..^1]);

                var last = song.Lines[^1];
                lines.Add(last with { IsLast = true });
            }

            var new_num = song.Lines.Max(a => a.LineNumber) + 1;
            var continuous_new_num = song.Lines.Max(a => a.ContinuousNumber) + 1;

            if (parameters.OverlapChapters && next_song is not null)
            {
                var line_end = next_song.Lines.First();
                var text_end = line_end.Text;
                lines.Add(new(new_num, continuous_new_num, new_num, text_end, LineType.NextSong, false, false, line_end.NotMy));
            }

            yield return song with { Lines = [.. lines] };
        }
    }

    private static IEnumerable<MyLine[]> ChunksWithOverlap(MyLine[] lines, int chunk_size)
    {
        var result = new List<MyLine>();

        for (var i = 0; i < lines.Length; ++i)
        {
            var line = lines[i];

            if (result.Count + 1 == chunk_size)
            {
                //Add last line of a chunk. Only mark it as Last if it is not the first line of the next Song
                result.Add(line with { IsLast = (line.LineType == LineType.Norm) });

                //Add the first line of the next Chunk
                if (i < lines.Length - 1)
                    result.Add(lines[i + 1] with { LineType = LineType.NextPage });

                yield return result.ToArray();

                result.Clear();

                //add line again for next chunk
                result.Add(line with { LineType = LineType.PrevPage });
            }
            else
            {
                result.Add(line with { IsFirst = (result.Count == 1) });
            }
        }

        if (result.Count > 1)
            yield return result.ToArray();
    }

    private static string CreateHeader(MySection section, MySong song, int screen_number)
    {
        string[] elements = [section.SectionName, song.SongName];
        var title = String.Join(", ", elements.Where(a => !String.IsNullOrEmpty(a)));
        return $"{title} ({(screen_number + 1)})";
    }
}

public record Chunk(string Header, int SectionNumber, string SectionName, int SongNumber, int ScreenNumber, int ChunkNumber, MyLine[] Lines);
