namespace AnkiPoetry.Engine;

public record MyDocument(MySection[] Sections, int MaxSongNumber);
public record MySection(int SectionNumber, string SectionName, MySong[] Songs);
public record MySong(int SongNumber, string SongName, MyLine[] Lines);
public record MyLine(int LineNumber, int ContinousNumber, string Text, LineType LineType, bool IsLast);
public record Card(string Number, string Text);

public enum LineType { Norm, PrevPage, NextPage, PrevSong, NextSong };

public record Parameters
{
    public string DeckName { get; set; } = "";
    public int ChunkSize { get; set; } = 20;
    public int WordWrap { get; set; } = -1;
    public bool WrapOnSpaces { get; set; } = true;
    public bool AddDots { get; set; } = false;
    public int Colors { get; set; } = 6;
    public bool OverlapChapters { get; set; } = true;
    public bool EmptyEndElement { get; set; } = true;
    public bool TitleToBegin { get; set; } = true;
    public bool LineNumbers { get; set; } = true;
    public bool Continous { get; set; } = true;
}
