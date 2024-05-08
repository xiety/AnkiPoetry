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

        var parameters = new Parameters();
        var doc = LoaderText.LoadText(text, parameters);
        var chunks = Chunker.Run(doc, parameters);

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

        var parameters = new Parameters();
        var doc = LoaderText.LoadText(text, parameters);
        var chunks = Chunker.Run(doc, parameters);

        Assert.AreEqual(1, chunks.Length);
    }
}
