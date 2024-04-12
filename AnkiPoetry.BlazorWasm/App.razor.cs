using AnkiPoetry.Engine;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AnkiPoetry.BlazorWasm;

public sealed partial class App : IAsyncDisposable
{
    [Inject]
    public required IJSRuntime JS { get; set; }

    [Inject]
    public required ISyncLocalStorageService LocalStorage { get; set; }

    const string ParametersKey = "AnkiPoetry:Parameters";

    private readonly CreatorInfo[] infos =
    [
        new(new WordCreator(), "word", "Word", "1. word.csv"),
        new(new LineCreator(), "line", "Line", "2. line.csv"),
        new(new PageCreator(), "page", "Page", "3. page.csv"),
    ];

    private readonly SampleCreator creator_sample = new();

    //mutable
    private IJSObjectReference? module;
    private Parameters parameters = new();
    private Card[] samples = [];

    protected override void OnInitialized()
    {
        Parameters? saved;

        try
        {
            saved = LocalStorage.GetItem<Parameters>(ParametersKey);
        }
        catch
        {
            saved = null;
        }

        if (saved is null)
        {
            LoadHamlet();
        }
        else
        {
            parameters = saved;
            Generate();
        }
    }

    private void LoadHamlet()
    {
        parameters.Text = Samples.HamletText;
        parameters.ChunkSize = 20;
        parameters.WordWrap = -1;
        parameters.Colors = 6;

        Generate();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./App.razor.js");
    }

    private void Generate()
    {
        LocalStorage.SetItem(ParametersKey, parameters);

        var doc = LoaderText.LoadLines(parameters.Text.Replace("\r\n", "\n").Split("\n"), parameters.WordWrap, parameters.WrapOnSpaces);
        var chunks = Chunker.Run(doc, parameters.ChunkSize, parameters.OverlapChapters).ToArray();

        samples = creator_sample.Run(chunks, parameters.Colors);

        foreach (var info in infos)
        {
            var cards = info.Creator.Run(chunks, parameters.Colors);
            info.Csv = CsvSaver.CreateCsv(cards, [info.NoteType]);
        }
    }

    private async Task DownloadContent(string element_id, string file_name)
    {
        if (module is not null)
            await module.InvokeVoidAsync("downloadContent", element_id, file_name);
    }

    public async ValueTask DisposeAsync()
    {
        if (module is not null)
            await module.DisposeAsync();
    }

    record CreatorInfo(BaseCreator<Card> Creator, string Id, string Title, string FileName)
    {
        public string Csv { get; set; } = "";
        public string ElementId => $"text_{Id}";
        public string NoteType => $"#notetype:poetry::{Id}";
    }

    record Parameters
    {
        public int ChunkSize { get; set; }
        public int WordWrap { get; set; }
        public bool WrapOnSpaces { get; set; }
        public int Colors { get; set; }
        public bool OverlapChapters { get; set; } = true;

        public string Text { get; set; } = "";
    }
}
