namespace AnkiPoetry.Engine;

public static class Chunker
{
    public static Chunk[] Run(MyDocument doc, int chunk_size, bool overlap_chapters, bool empty_end_element, bool title_to_begin)
        => RunEnumerable(doc, chunk_size, overlap_chapters, empty_end_element, title_to_begin).ToArray();

    public static IEnumerable<Chunk> RunEnumerable(MyDocument doc, int chunk_size, bool overlap_chapters, bool empty_end_element, bool title_to_begin)
    {
        foreach (var section in doc.Sections)
        {
            foreach (var song in Augmented(section, overlap_chapters, empty_end_element, title_to_begin))
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

    private static IEnumerable<MySong> Augmented(MySection section, bool overlap_chapters, bool empty_end_element, bool title_to_begin)
    {
        for (var i = 0; i < section.Songs.Length; ++i)
        {
            var lines = new List<MyLine>();

            var title = !String.IsNullOrEmpty(section.Songs[i].SongName) ? section.Songs[i].SongName : section.SectionName;
            var text_begin = (i != 0 && overlap_chapters) ? section.Songs[i - 1].Lines.Last().Text : (title_to_begin ? title : "");
            lines.Add(new(0, text_begin, LineType.Prev, false));

            lines.AddRange(section.Songs[i].Lines[..^1]);

            var last = section.Songs[i].Lines[^1];
            lines.Add(last with { IsLast = true });

            var new_num = section.Songs[i].Lines.Max(a => a.LineNumber) + 1;

            if (overlap_chapters && i != section.Songs.Length - 1)
            {
                var text_end = section.Songs[i + 1].Lines.First().Text;
                lines.Add(new(new_num, text_end, LineType.Next, false));
            }
            else if (empty_end_element)
            {
                lines.Add(new(new_num, "", LineType.Next, false));
            }

            yield return section.Songs[i] with { Lines = [.. lines] };
        }
    }

    private static IEnumerable<MyLine[]> ChunksWithOverlap(IEnumerable<MyLine> lines, int chunk_size)
    {
        var result = new List<MyLine>();

        foreach (var line in lines)
        {
            if (result.Count + 1 == chunk_size)
            {
                result.Add(line with { IsLast = (line.LineType == LineType.Norm) });

                yield return result.ToArray();

                result.Clear();
                result.Add(line);
            }
            else
            {
                result.Add(line);
            }
        }

        if (result.Count > 1)
            yield return result.ToArray();
    }

    private static string CreateHeader(MySection section, MySong song, int screenNumber)
    {
        string[] elements = [section.SectionName, song.SongName];
        var title = String.Join(", ", elements.Where(a => !String.IsNullOrEmpty(a)));
        return $"{title} ({screenNumber})";
    }
}

public record Chunk(int MaxSongNumber, string Header, int SectionNumber, int SongNumber, MyLine[] Lines);
