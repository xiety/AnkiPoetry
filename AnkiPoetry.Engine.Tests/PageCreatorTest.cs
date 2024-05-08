namespace AnkiPoetry.Engine.Tests;

[TestClass]
public class PageCreatorTest
{
    [TestMethod]
    public void TestNormal()
    {
        var pageCreator = new PageCreator();

        var text = """
            # Test
            A
            B
            C
            D
            E
            """;

        var parameters = new Parameters { ChunkSize = 5, TitleToBegin = false };
        var doc = LoaderText.LoadText(text, parameters);
        var chunks = Chunker.Run(doc, parameters);
        var cards = pageCreator.Run(chunks, parameters);

        Assert.AreEqual(2, cards.Length);

        Assert.AreEqual("""<div class="header">Test (1)</div><br><div class="line0">  0. {{c1::(Begin)}}</div><div class="line1">  1. {{c2::A}}</div><div class="line2">  2. {{c1::B}}</div><div class="line3">  3. {{c2::C}}</div><div class="line4">  4. {{c1::D}}</div><hr>""", cards[0].Text);
        Assert.AreEqual("""<div class="header">Test (2)</div><br><div class="line4">  4. {{c1::D}}</div><div class="line5">  5. {{c2::E}}</div><hr><div class="line0">  6. {{c1::(End)}}</div>""", cards[1].Text);
    }

    [TestMethod]
    public void TestOverlap()
    {
        var pageCreator = new PageCreator();

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

        var parameters = new Parameters { ChunkSize = 5, TitleToBegin = false, Continous = false };
        var doc = LoaderText.LoadText(text, parameters);
        var chunks = Chunker.Run(doc, parameters);
        var cards = pageCreator.Run(chunks, parameters);

        Assert.AreEqual(2, cards.Length);

        Assert.AreEqual("""<div class="header">Test 1 (1)</div><br><div class="line0">  0. {{c1::(Begin)}}</div><div class="line1">  1. {{c2::A}}</div><div class="line2">  2. {{c1::B}}</div><div class="line3">  3. {{c2::C}}</div><hr><div class="line4">  4. {{c1::(End) D}}</div>""", cards[0].Text);
        Assert.AreEqual("""<div class="header">Test 2 (1)</div><br><div class="line0">  0. {{c1::C (Begin)}}</div><div class="line1">  1. {{c2::D}}</div><div class="line2">  2. {{c1::E}}</div><div class="line3">  3. {{c2::F}}</div><hr><div class="line4">  4. {{c1::(End)}}</div>""", cards[1].Text);
    }
}
