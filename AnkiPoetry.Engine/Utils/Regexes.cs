using System.Text.RegularExpressions;

namespace AnkiPoetry.Engine;

public static partial class Regexes
{
    //this regex matches words that may contain apostrophes within them
    [GeneratedRegex(@"\b(?:['`’]\b|\B['`’]\B)?\w+(?:(['`’])\w+)*\b")]
    public static partial Regex RegexWord();
}
