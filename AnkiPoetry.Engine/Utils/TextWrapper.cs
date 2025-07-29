namespace AnkiPoetry.Engine;

public static class TextWrapper
{
    public static IEnumerable<string> Wrap(string text, Parameters parameters)
    {
        if (parameters.WordWrap == -1)
        {
            yield return text;
            yield break;
        }

        var matches = Regexes.RegexWord().Matches(text);
        if (matches.Count == 0)
        {
            yield return text;
            yield break;
        }

        var start = matches[0].Index;
        var longWordCount = 0;

        for (var i = 0; i < matches.Count; ++i)
        {
            if (matches[i].Value.Length >= 4)
                longWordCount++;

            var isLast = i == matches.Count - 1;
            var shouldWrap = longWordCount == parameters.WordWrap || isLast;

            if (shouldWrap)
            {
                var end = isLast ? text.Length : matches[i + 1].Index;
                yield return text[start..end].TrimEnd();
                start = end;
                longWordCount = 0;
            }
        }
    }
}
