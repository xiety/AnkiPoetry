namespace AnkiPoetry.Engine.Tests;

[TestClass]
public class LineCreatorTest
{
    [TestMethod]
    public void TestNormal()
    {
        var lineCreator = new LineCreator();

        var text = """
            # Test
            A
            B
            C
            D
            E
            """;

        var parameters = new Parameters { OverlapChapters = false, TitleToBegin = false, ChunkSize = 5 };
        var doc = LoaderText.LoadText(text, parameters);
        var chunks = Chunker.Run(doc, parameters);
        var cards = lineCreator.Run(chunks, parameters);

        Assert.AreEqual(6, cards.Length);

        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. {{c1::A}}</div>""", cards[0].Text, message: "0");
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. {{c1::B}}</div>""", cards[1].Text, message: "1");
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. {{c1::C}}</div>""", cards[2].Text, message: "2");
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. C</div><div class="line4">  4. {{c1::D}}</div><hr>""", cards[3].Text, message: "3");
        Assert.AreEqual("""<div class="header">Test (2)</div><br><div class="line4">  4. D</div><div class="line5">  5. {{c1::E}}</div><hr>""", cards[4].Text, message: "4");
        Assert.AreEqual("""<div class="header">Test (2)</div><br><div class="line4">  4. D</div><div class="line5">  5. E</div><hr><div class="line0">  6. {{c1::(End)}}</div>""", cards[5].Text, message: "5");
    }

    [TestMethod]
    public void TestOverlap()
    {
        var lineCreator = new LineCreator();

        var text = """
            ## Test 1
            A
            B
            C
            ## Test 2
            D
            E
            F
            """;

        var parameters = new Parameters { OverlapChapters = true, TitleToBegin = false, ChunkSize = 5, Continous = false };
        var doc = LoaderText.LoadText(text, parameters);
        var chunks = Chunker.Run(doc, parameters);
        var cards = lineCreator.Run(chunks, parameters);

        Assert.AreEqual(8, cards.Length);

        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. {{c1::A}}</div>""", cards[0].Text, message: "0");
        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. {{c1::B}}</div>""", cards[1].Text, message: "1");
        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. {{c1::C}}</div><hr>""", cards[2].Text, message: "2");
        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. C</div><hr><div class="line4">  4. {{c1::(End) D}}</div>""", cards[3].Text, message: "3");
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. {{c1::D}}</div>""", cards[4].Text, message: "4");
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. D</div><div class="line2">  2. {{c1::E}}</div>""", cards[5].Text, message: "5");
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. D</div><div class="line2">  2. E</div><div class="line3">  3. {{c1::F}}</div><hr>""", cards[6].Text, message: "6");
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. D</div><div class="line2">  2. E</div><div class="line3">  3. F</div><hr><div class="line4">  4. {{c1::(End)}}</div>""", cards[7].Text, message: "7");
    }
}
