﻿namespace AnkiPoetry.Engine;

public static class TextWrapper
{
    public static string[] Wrap(string text, Parameters parameters)
    {
        if (parameters.WordWrap == -1 || text.Length <= parameters.WordWrap)
            return [text];

        var lines = parameters.WrapOnSpaces
            ? WrapOnSpaces(text, parameters.WordWrap).ToArray()
            : WrapOnPunctuation(text, parameters.WordWrap).ToArray();

        return parameters.AddDots
            ? AddDots(lines).ToArray()
            : lines;
    }

    private static IEnumerable<string> WrapOnPunctuation(string text, int word_wrap)
    {
        var items = Split(text);

        var c = "";

        foreach (var item in items)
        {
            var n = c + item;

            if (n.Length > word_wrap)
            {
                if (c != "")
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

    private static IEnumerable<string> WrapOnSpaces(string text, int word_wrap)
    {
        var items = text.Split(' ');

        var c = "";

        foreach (var item in items)
        {
            var n = (c == "") ? item : c + " " + item;

            if (n.Length > word_wrap)
            {
                if (c != "")
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
