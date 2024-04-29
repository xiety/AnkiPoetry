namespace AnkiPoetry.Engine;

public record MyDocument(MySection[] Sections, int MaxSongNumber);
public record MySection(int SectionNumber, string SectionName, MySong[] Songs);
public record MySong(int SongNumber, string SongName, MyLine[] Lines);
public record MyLine(int LineNumber, string Text, LineType LineType, bool IsLast);
public record Card(string Number, string Text);

public enum LineType { Norm, Prev, Next };
