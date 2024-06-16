using System.Text.Json;

using AnkiPoetry.Engine;

switch (args)
{
    case [string parameters_file, string text_file, string output_folder]:
        Create(parameters_file, text_file, output_folder);
        break;
    default:
        Console.WriteLine("Args: parameters_file text_file output_folder");
        break;
}

static void Create(string parameters_file, string text_file, string output_folder)
{
    var parameters = JsonSerializer.Deserialize<Parameters>(File.ReadAllText(parameters_file))!;
    var text = File.ReadAllText(text_file);

    CreatorInfo[] infos =
    [
        new(new WordCreator(), "word", "1. word"),
        new(new LineCreator(), "line", "2. line"),
        new(new PageCreator(), "page", "3. page"),
    ];

    var doc = LoaderText.LoadText(text, parameters);

    var chunks = Chunker.Run(doc, parameters);

    if (!Directory.Exists(output_folder))
        Directory.CreateDirectory(output_folder);

    foreach (var info in infos)
    {
        var cards = info.Creator.Run(chunks, parameters);

        var csv = CsvSaver.CreateCsv(cards, [
            "#separator:semicolon",
            $"#notetype:poetry::{info.Id}",
            $"#deck:{parameters.DeckName}::{info.DeckName}"]);

        var full_file = Path.Combine(output_folder, info.DeckName + ".csv");

        File.WriteAllText(full_file, csv);
    }
}

record CreatorInfo(BaseCreator<Card> Creator, string Id, string DeckName);
