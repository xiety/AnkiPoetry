namespace AnkiPoetry.Engine.Tests;

[TestClass]
public class ChunkerTest
{
    [TestMethod]
    public void DoesNotCreatePageWithSingleLine()
    {
        var text = """
            1
            2
            3
            4
            """;

        var doc = LoaderText.LoadText(text, -1, false, false);

        var chunks = Chunker.Run(doc, 5, true, false, true);

        Assert.AreEqual(1, chunks.Length);
    }

    [TestMethod]
    public void DoesNotCreatePageWithSingleLine_WithEmptyEndElement()
    {
        var text = """
            1
            2
            3
            """;

        var doc = LoaderText.LoadText(text, -1, false, false);

        var chunks = Chunker.Run(doc, 5, true, empty_end_element: true, true);

        Assert.AreEqual(1, chunks.Length);
    }
}
