namespace AnkiPoetry.Engine;

public record MyDocument(MySection[] Sections);
public record MySection(int SectionNumber, string SectionName, MySong[] Songs);
public record MySong(int SongNumber, string SongName, MyLine[] Lines);
public record MyLine(int LineNumber, int ContinuousNumber, int NumberForColor, string Text, LineType LineType, bool IsFirst, bool IsLast, bool NotMy);
public record Card(string Number, string Text);

public enum LineType { Norm, PrevPage, NextPage, PrevSong, NextSong };

public record Parameters
{
    public string DeckName { get; set; } = "";
    public int ChunkSize { get; set; } = 20;
    public int WordWrap { get; set; } = -1;
    public int Colors { get; set; } = 7;
    public bool OverlapChapters { get; set; } = true;
    public bool LineNumbers { get; set; } = true;
    public bool Continuous { get; set; } = true;
    public StarMode StarMode { get; set; } = StarMode.PerChunk;
    public int StarNumber { get; set; } = 5;
}

public enum StarMode
{
    None,
    PerChunk,
    PerSection,
    PerSong,
}
