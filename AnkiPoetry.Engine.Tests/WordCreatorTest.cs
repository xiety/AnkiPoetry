namespace AnkiPoetry.Engine.Tests;

[TestClass]
public class WordCreatorTest
{
    [TestMethod]
    public void TestNormal()
    {
        var wordCreator = new WordCreator();

        var text = """
            # Test
            A
            B
            C
            D
            E
            """;

        var parameters = new Parameters { ChunkSize = 5, TitleToBegin = false, Continous = false };
        var doc = LoaderText.LoadText(text, parameters);
        var chunks = Chunker.Run(doc, parameters);
        var cards = wordCreator.Run(chunks, parameters);

        Assert.AreEqual(5, cards.Length);

        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. {{c1::A}}</div>""", cards[0].Text, message: "0");
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. {{c1::B}}</div>""", cards[1].Text, message: "1");
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. {{c1::C}}</div>""", cards[2].Text, message: "2");
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. C</div><div class="line4">  4. {{c1::D}}</div><hr>""", cards[3].Text, message: "3");
        Assert.AreEqual("""<div class="header">Test (2)</div><br><div class="line4">  4. D</div><div class="line5">  5. {{c1::E}}</div><hr>""", cards[4].Text, message: "4");
    }
}
