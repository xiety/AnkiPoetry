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

        var doc = LoaderText.LoadText(text, -1, false, false);
        var chunks = Chunker.Run(doc, 5, true, true, false);

        var cards = lineCreator.Run(chunks, 6, true);

        Assert.AreEqual(6, cards.Length);

        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. {{c1::A}}</div>""", cards[0].Text);
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. {{c1::B}}</div>""", cards[1].Text);
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. {{c1::C}}</div>""", cards[2].Text);
        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. C</div><div class="line4">  4. {{c1::D}}</div>""", cards[3].Text);
        Assert.AreEqual("""<div class="header">Test (2)</div><br><div class="line4">  4. D</div><div class="line5">  5. {{c1::E}}</div>""", cards[4].Text);
        Assert.AreEqual("""<div class="header">Test (2)</div><br><div class="line4">  4. D</div><div class="line5">  5. E</div><div class="line0">  6. {{c1::(End)}}</div>""", cards[5].Text);
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

        var doc = LoaderText.LoadText(text, -1, false, false);
        var chunks = Chunker.Run(doc, 5, true, true, false);

        var cards = lineCreator.Run(chunks, 6, true);

        Assert.AreEqual(8, cards.Length);

        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. {{c1::A}}</div>""", cards[0].Text);
        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. {{c1::B}}</div>""", cards[1].Text);
        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. {{c1::C}}</div>""", cards[2].Text);
        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. (Begin)</div><div class="line1">  1. A</div><div class="line2">  2. B</div><div class="line3">  3. C</div><div class="line4">  4. {{c1::(End) D}}</div>""", cards[3].Text);
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. {{c1::D}}</div>""", cards[4].Text);
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. D</div><div class="line2">  2. {{c1::E}}</div>""", cards[5].Text);
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. D</div><div class="line2">  2. E</div><div class="line3">  3. {{c1::F}}</div>""", cards[6].Text);
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. C (Begin)</div><div class="line1">  1. D</div><div class="line2">  2. E</div><div class="line3">  3. F</div><div class="line4">  4. {{c1::(End)}}</div>""", cards[7].Text);
    }
}
