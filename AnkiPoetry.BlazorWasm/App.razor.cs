using System.Text.Json;

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

    const string StateKey = "AnkiPoetry:State";

    private readonly CreatorInfo[] infos =
    [
        new(new WordCreator(), "word", "Word", "1. word"),
        new(new LineCreator(), "line", "Line", "2. line"),
        new(new PageCreator(), "page", "Page", "3. page"),
    ];

    private readonly SampleCreator creator_sample = new();

    //mutable
    private IJSObjectReference? module;
    private State state = new();
    private Card[] samples = [];

    protected override void OnInitialized()
    {
        State? savedState;

        try
        {
            savedState = LocalStorage.GetItem<State>(StateKey);
        }
        catch
        {
            savedState = null;
        }

        if (savedState is null)
        {
            LoadHamlet();
        }
        else
        {
            state = savedState;
            Generate();
        }
    }

    private void LoadHamlet()
    {
        state.Text = Samples.HamletText;
        state.Parameters.ChunkSize = 20;
        state.Parameters.WordWrap = -1;
        state.Parameters.Colors = 6;

        Generate();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./App.razor.js");
    }

    private void Generate()
    {
        LocalStorage.SetItem(StateKey, state);

        var doc = LoaderText.LoadText(state.Text, state.Parameters);

        var chunks = Chunker.Run(doc, state.Parameters);

        samples = creator_sample.Run(chunks, state.Parameters);

        foreach (var info in infos)
        {
            var cards = info.Creator.Run(chunks, state.Parameters);
            info.Csv = CsvSaver.CreateCsv(cards, [
                "#separator:semicolon",
                $"#notetype:poetry::{info.Id}",
                $"#deck:{state.Parameters.DeckName}::{info.DeckName}"]);
        }
    }

    private async Task DownloadContent(string element_id, string file_name)
    {
        if (module is not null)
            await module.InvokeVoidAsync("downloadContent", element_id, file_name);
    }

    private string GetJson() => JsonSerializer.Serialize(state.Parameters);

    private void SetJson(string? newValue)
    {
        try
        {
            state.Parameters = JsonSerializer.Deserialize<Parameters>(newValue!)!;
        }
        catch
        {
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (module is not null)
            await module.DisposeAsync();
    }

    record CreatorInfo(BaseCreator<Card> Creator, string Id, string Title, string DeckName)
    {
        public string Csv { get; set; } = "";
        public string ElementId => $"text_{Id}";
        public string FileName => DeckName + ".csv";
    }

    record State
    {
        public string Text { get; set; } = "";
        public Parameters Parameters { get; set; } = new();
    }
}
